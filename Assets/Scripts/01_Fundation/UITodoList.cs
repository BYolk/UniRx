using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class UITodoList : MonoBehaviour
{
    #region

    #endregion
    public Note.TodoList todoList;

    UIToDoItem _todoItemPrototype;
    UIInputController _inputController;
    Transform _contentTransfrom;
    Image _eventMaskImage;
    
    private void Awake()
    {
        //�ҵ�ǰ transform ���Ӻ��ӣ������ҵ����ɼ��ĺ��ӣ�������� GameObject ���Ҳ�����GameObject.Find �Ҳ������ɼ��ĺ���
        _todoItemPrototype = transform.Find("TodoItemPrototype").GetComponent<UIToDoItem>();
        _inputController = transform.Find("InputController").GetComponent<UIInputController>();
        _contentTransfrom = GameObject.Find("Content").transform;//���� transform �Ҳ�����transfrom.Find Ҫһ��һ��������
        _eventMaskImage = transform.Find("EventMask_Image").GetComponent<Image>();
         
        todoList = Note.TodoList.Load();
        _inputController.todoList = todoList;
    }

    private void Start()
    {
        _inputController.mode.Subscribe(mode =>
        {
            if(mode == UIInputController.Mode.Add)
                _eventMaskImage.gameObject.SetActive(false);
            else
                _eventMaskImage.gameObject.SetActive(true);
        });
        //ע�Ἧ��ֵ�����ı���¼����� todoItems ֵ�����ı�ʱ����ˢ�б�
        todoList.todoItems.ObserveEveryValueChanged(todoItems => todoItems.Count).Subscribe(_ => OnDataChange());
        OnDataChange();
    }

    void OnDataChange()
    {
        int childCount = _contentTransfrom.childCount;
        for(int i = 0; i < childCount; i++)//���ÿһ��
            Destroy(_contentTransfrom.GetChild(i).gameObject);

        todoList.todoItems
            .Where(todoItem => !todoItem.Completed.Value)
            .ToList()
            .ForEach(todoItem =>
            {
                if (!todoItem.Completed.Value)//���� item �� Complete Ϊ True �� Item
                {
                    todoItem.Completed.Subscribe(completed =>
                    {
                        if (completed)
                            OnDataChange();
                    });//Ϊÿ����ť����Ӧʽ���� Completed ע��ص��¼����� Completed ��ֵ�����ı�ʱ��ִ�лص�
                    UIToDoItem uiTodoItem = Instantiate(_todoItemPrototype);
                    uiTodoItem.transform.SetParent(_contentTransfrom);
                    uiTodoItem.transform.localScale = Vector3.one;
                    uiTodoItem.gameObject.SetActive(true);
                    uiTodoItem.SetModel(todoItem);

                    uiTodoItem.itemBtn.OnClickAsObservable()//���ÿһ�����༭ģʽ
                        .Subscribe(_ =>
                        {
                            _inputController.EditMode(todoItem);
                            
                        });
                }
            });
        todoList.Save();
    }
}
