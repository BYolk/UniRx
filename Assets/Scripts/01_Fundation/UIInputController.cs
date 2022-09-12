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
    Note.TodoItem _cachedItem;//内容的缓存
    public Note.TodoList todoList;
    public ReactiveProperty<Mode> mode = new ReactiveProperty<Mode>(Mode.Add);//此处老师使用 UniRxLesson.Mode.Add，但报错找不到，使用 Mode.Add 无效果

    private void Awake()
    {
        _inputField = transform.Find("Content_InputField").GetComponent<InputField>();
        _addItemButton = transform.Find("AddItem_Button").GetComponent<Button>();
        _updateItemButton = transform.Find("UpdateItem_Button").GetComponent<Button>();
        _cancelButton = transform.Find("Cancel_Button").GetComponent<Button>();
    }

    private void Start()
    {
        _inputField.OnValueChangedAsObservable()//输入框有值发生改变时
            .Select(inputContent => !string.IsNullOrEmpty(inputContent))//如果输入框值不为空，返回 true，为空返回 false
            .SubscribeToInteractable(_addItemButton);//将返回布尔值和 _addItemButton 按钮的可交互性绑定到一起

        _inputField.OnValueChangedAsObservable()
            .Select(inputContent => _cachedItem != null && inputContent != _cachedItem.Content.Value && !string.IsNullOrEmpty(inputContent))
            .SubscribeToInteractable(_updateItemButton);

        _addItemButton.OnClickAsObservable()//注册按钮点击事件
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
        _cachedItem = todoItem;//缓存原先的内容（如果内容没有改变，不能进行更新操作，不可以进行更新按钮的点击）
        _inputField.text = todoItem.Content.Value;
        _addItemButton.gameObject.SetActive(false);
        _updateItemButton.gameObject.SetActive(true);
        _cancelButton.gameObject.SetActive(true);
    }
}
