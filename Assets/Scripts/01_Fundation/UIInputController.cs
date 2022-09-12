using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIInputController : MonoBehaviour
{
    public enum Mode
    {
        Add,
        Edit
    }

    #region

    #endregion
    InputField _inputField;
    Button _addItemButton;
    Button _updateItemButton;
    Button _cancelButton;
    Note.TodoItem _cachedItem;//���ݵĻ���
    public Note.TodoList todoList;
    public ReactiveProperty<Mode> mode = new ReactiveProperty<Mode>(Mode.Add);//�˴���ʦʹ�� UniRxLesson.Mode.Add���������Ҳ�����ʹ�� Mode.Add ��Ч��

    private void Awake()
    {
        _inputField = transform.Find("Content_InputField").GetComponent<InputField>();
        _addItemButton = transform.Find("AddItem_Button").GetComponent<Button>();
        _updateItemButton = transform.Find("UpdateItem_Button").GetComponent<Button>();
        _cancelButton = transform.Find("Cancel_Button").GetComponent<Button>();
    }

    private void Start()
    {
        _inputField.OnValueChangedAsObservable()//�������ֵ�����ı�ʱ
            .Select(inputContent => !string.IsNullOrEmpty(inputContent))//��������ֵ��Ϊ�գ����� true��Ϊ�շ��� false
            .SubscribeToInteractable(_addItemButton);//�����ز���ֵ�� _addItemButton ��ť�Ŀɽ����԰󶨵�һ��

        _inputField.OnValueChangedAsObservable()
            .Select(inputContent => _cachedItem != null && inputContent != _cachedItem.Content.Value && !string.IsNullOrEmpty(inputContent))
            .SubscribeToInteractable(_updateItemButton);

        _addItemButton.OnClickAsObservable()//ע�ᰴť����¼�
            .Subscribe(_ =>
            {
                todoList.Add(_inputField.text);
                _inputField.text = string.Empty;
            });

        _updateItemButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _cachedItem.Content.Value = _inputField.text;
                AddMode();
            });

        _cancelButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                AddMode();
            });
    }


    void AddMode()
    {
        mode.Value = Mode.Add;
        _cachedItem = null;
        _inputField.text = string.Empty;
        _addItemButton.gameObject.SetActive(true);
        _updateItemButton.gameObject.SetActive(false);
        _cancelButton.gameObject.SetActive(false);
    }

    public void EditMode(Note.TodoItem todoItem)
    {
        mode.Value = Mode.Edit;
        _cachedItem = todoItem;//����ԭ�ȵ����ݣ��������û�иı䣬���ܽ��и��²����������Խ��и��°�ť�ĵ����
        _inputField.text = todoItem.Content.Value;
        _addItemButton.gameObject.SetActive(false);
        _updateItemButton.gameObject.SetActive(true);
        _cancelButton.gameObject.SetActive(true);
    }
}
