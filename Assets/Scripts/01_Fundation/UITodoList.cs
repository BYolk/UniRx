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
        //找当前 transform 的子孩子，可以找到不可见的孩子，如果换成 GameObject 会找不到，GameObject.Find 找不到不可见的孩子
        _todoItemPrototype = transform.Find("TodoItemPrototype").GetComponent<UIToDoItem>();
        _inputController = transform.Find("InputController").GetComponent<UIInputController>();
        _contentTransfrom = GameObject.Find("Content").transform;//换成 transform 找不到，transfrom.Find 要一层一层往下找
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
        //注册集合值发生改变的事件，当 todoItems 值发生改变时，重刷列表
        todoList.todoItems.ObserveEveryValueChanged(todoItems => todoItems.Count).Subscribe(_ => OnDataChange());
        OnDataChange();
    }

    void OnDataChange()
    {
        int childCount = _contentTransfrom.childCount;
        for(int i = 0; i < childCount; i++)//清除每一项
            Destroy(_contentTransfrom.GetChild(i).gameObject);

        todoList.todoItems
            .Where(todoItem => !todoItem.Completed.Value)
            .ToList()
            .ForEach(todoItem =>
            {
                if (!todoItem.Completed.Value)//过滤 item 中 Complete 为 True 的 Item
                {
                    todoItem.Completed.Subscribe(completed =>
                    {
                        if (completed)
                            OnDataChange();
                    });//为每个按钮的响应式属性 Completed 注册回调事件，当 Completed 的值发生改变时，执行回调
                    UIToDoItem uiTodoItem = Instantiate(_todoItemPrototype);
                    uiTodoItem.transform.SetParent(_contentTransfrom);
                    uiTodoItem.transform.localScale = Vector3.one;
                    uiTodoItem.gameObject.SetActive(true);
                    uiTodoItem.SetModel(todoItem);

                    uiTodoItem.itemBtn.OnClickAsObservable()//点击每一项进入编辑模式
                        .Subscribe(_ =>
                        {
                            _inputController.EditMode(todoItem);
                            
                        });
                }
            });
        todoList.Save();
    }
}
