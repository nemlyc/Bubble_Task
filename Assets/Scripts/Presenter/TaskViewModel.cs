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
    readonly string UpdateReturnTrue = "タスクを更新しました。";
    readonly string ReturnEmpty = "入力が空欄です。";
    readonly string ReturnFillOver = "文字数オーバーです。";

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
        var result = ValidateEntity(description, node, out var validateResult);
        if (result)
        {
            taskModel.CreateTask(user, priority, description, node);

            resultText = CreateReturnTrue;
            return true;
        }
        else
        {
            resultText = validateResult;
            return false;
        }
    }
    public bool UpdateTask(int priority, string description, List<string> node, out string resultText)
    {
        var result = ValidateEntity(description, node, out var validateResult);
        if (result)
        {
            //Todo: 冗長
            taskModel.UpdatePriority(FocusID.Value, priority);
            taskModel.UpdateDescription(FocusID.Value, description);
            //taskModel.UpdateNodeIDs(FocusID.Value, node);

            resultText = UpdateReturnTrue;
            return true;
        }
        else
        {
            resultText = validateResult;
            return false;
        }
    }

    public bool ReadTask(out TaskEntity entity)
    {
        if (focusID.Value != null)
        {
            entity = taskModel.ReadTask(focusID.Value);
            return true;
        }

        entity = null;
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

    private bool ValidateEntity(string description, List<string> node, out string resultText)
    {
        if (description.Length == 0)
        {
            resultText = ReturnEmpty;
            return false;
        }
        if (description.Length >= BodyTextMaxLength)
        {
            var overSize = description.Length - BodyTextMaxLength;
            resultText = $"{ReturnFillOver} : {overSize}";
            return false;
        }

        resultText = CreateReturnTrue;
        return true;
    }
}
