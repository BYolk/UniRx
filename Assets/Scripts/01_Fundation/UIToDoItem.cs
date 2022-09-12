using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIToDoItem : MonoBehaviour
{
    #region

    #endregion
    Button _completedBtn;
    public Button itemBtn;
    Text _content;
    Note.TodoItem _todoItem;

    private void Awake()
    {
        itemBtn = GetComponent<Button>();
        _content = transform.Find("Content_Text").GetComponent<Text>();
        _completedBtn = transform.Find("Completed_Button").GetComponent<Button>();
        _completedBtn.OnClickAsObservable().Subscribe(_ =>
        {
            _todoItem.Completed.Value = true;
        });

        
    }

    public void SetModel(Note.TodoItem todoItem)
    {
        _todoItem = todoItem;
        UpdateView(_todoItem.Content.Value);
        _todoItem.Content.Subscribe(UpdateView).AddTo(this);
    }

    void UpdateView(string content)
    {
        _content.text = content;
    }
}
