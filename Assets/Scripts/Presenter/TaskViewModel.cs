using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TaskViewModel : MonoBehaviour
{
    [SerializeField]
    TopView topView;
    [SerializeField]
    StaticView staticView;
    [SerializeField]
    EditorView editorView;

    [SerializeField]
    ViewStateProperty viewState = new ViewStateProperty(ViewState.Top);
    public IReadOnlyReactiveProperty<ViewState> ViewStatus { get { return viewState; } }

    ReactiveProperty<string> focusID = new ReactiveProperty<string>();
    public IReadOnlyReactiveProperty<string> FocusID { get { return focusID; } }

    string userID = "";

    TaskModel taskModel;
    UserEntity user;

    readonly int BodyTextMaxLength = 30;

    readonly string CreateReturnTrue = "タスクを登録しました。";
    readonly string CreateReturnNull = "入力が空欄です。";
    readonly string CreateReturnOver = "文字数オーバーです。";

    private void Awake()
    {
        taskModel = new TaskModel();
        user = new UserEntity();
    }

    private void Start()
    {
        InitializeData();

        this.viewState.Subscribe(x =>
        {
            staticView.UpdateHeader(x);
        })
        .AddTo(this);

        taskModel.AddDictionary.Subscribe(x =>
        {
            topView.AddBubble(taskModel.ReadTask(x.Key));
        }).AddTo(this);
        taskModel.RemoveDictionary.Subscribe(x =>
        {
            topView.DeleteBubble(x.Key);
        }).AddTo(this);

        taskModel.UpdateTaskInfo.Subscribe(task =>
        {
            topView.UpdateTask(task);
        }).AddTo(this);
        taskModel.CompleteTaskObserbable.Subscribe(task =>
        {
            topView.DeleteBubble(task.ID);
        }).AddTo(this);
    }

    public void SetView(ViewState state, string focusID = null)
    {
        viewState.Value = state;
        if (focusID != null)
        {
            this.focusID.Value = focusID;
        }
        else
        {
            this.focusID.Value = null;
        }
    }

    public void CompleteTask(string id)
    {
        taskModel.CompleteTask(id);        
    }

    public bool CreateTask(int priority, string description, List<string> node, out string resultText)
    {
        taskModel.CreateTask(user, priority, description, node);
        if (description.Length == 0)
        {
            resultText = CreateReturnNull;
            return false;
        }
        if (description.Length >= BodyTextMaxLength)
        {
            var overSize = description.Length - BodyTextMaxLength;
            resultText = $"{CreateReturnOver} : {overSize}";
            return false;
        }

        resultText = CreateReturnTrue;
        return true;
    }

    public bool ReadTask(out TaskEntity focusEntity)
    {
        if (FocusID.Value != null)
        {
            focusEntity = taskModel.ReadTask(FocusID.Value);
            return true;
        }

        focusEntity = null;
        return false;
    }

    public bool DeleteTask()
    {
        if (focusID.Value != null)
        {
            taskModel.DeleteTask(focusID.Value);
        }
        return true;
    }

    private void InitializeData()
    {
        //userIDを読み込む。
        //userID = "readID";
        user.SetID(userID);

        var taskDictionary = taskModel.ReadTaskDictionary();

        foreach (var item in taskDictionary.Keys)
        {
            topView.AddBubble(taskDictionary[item]);
        }
    }
}
