using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TMPro;
using UnityEngine.UI;

public class EditorView : MonoBehaviour
{
    [SerializeField]
    GameObject editorViewRoot;
    [SerializeField]
    Button[] sizeButtons;
    [SerializeField]
    GameObject sizeCursor;
    [SerializeField]
    Button submitButton;
    [SerializeField]
    Button deleteButton;
    [SerializeField]
    TMP_InputField taskInputField;

    [SerializeField]
    TaskViewModel taskViewModel;

    private IntReactiveProperty priority = new IntReactiveProperty();

    readonly int PriorityDefaultValue = 1;

    private void Start()
    {
        taskViewModel.ViewStatus.Subscribe(currentView =>
        {
            if (currentView.Equals(ViewState.Editor))
            {
                editorViewRoot.SetActive(true);
            }
            else
            {
                editorViewRoot.SetActive(false);
            }
        }).AddTo(this);

        InitializeSizeButtonEvents(sizeButtons);

        priority.Subscribe(buttonIndex =>
        {
            sizeCursor.transform.position = sizeButtons[buttonIndex].transform.position;
        }).AddTo(this);

        submitButton.OnClickAsObservable().Subscribe(_ =>
        {
            if (taskViewModel.FocusID.Value != null)
            {
                var result = taskViewModel.UpdateTask(priority.Value, taskInputField.text, null, out var resultText);
                if (result)
                {
                    // 成功ダイアログ
                    Debug.Log(resultText);

                    taskViewModel.SetView(ViewState.Top);
                }
                else
                {
                    // 失敗理由
                    Debug.Log(resultText);
                }
            }
            else
            {
                var result = taskViewModel.CreateTask(priority.Value, taskInputField.text, null, out var resultText);
                if (result)
                {
                    // 成功ダイアログ
                    Debug.Log(resultText);

                    taskViewModel.SetView(ViewState.Top);
                }
                else
                {
                    // 失敗理由
                    Debug.Log(resultText);
                }
            }
        }).AddTo(this);

        deleteButton.OnClickAsObservable().Subscribe(_ =>
        {
            var result = taskViewModel.DeleteTask();
            if (result)
            {
                taskViewModel.SetView(ViewState.Top);
            }
        }).AddTo(this);


        taskViewModel.FocusID.Subscribe(_ =>
        {
            UpdateEditorView();
        }).AddTo(this);
    }

    void UpdateEditorView()
    {
        var result = taskViewModel.ReadTask(out var entity);
        if (result)
        {
            priority.Value = entity.Priority;
            taskInputField.text = entity.Description;
        }
        else
        {
            priority.Value = PriorityDefaultValue;
            taskInputField.text = null;
        }
    }

    void InitializeSizeButtonEvents(Button[] buttons)
    {
        int i = 0;
        foreach (var item in buttons)
        {
            int index = i;

            item.OnClickAsObservable().Subscribe(_ =>{
                SetPriority(index);
            }).AddTo(this);

            i++;
        }
    }

    void SetPriority(int i)
    {
        priority.Value = i;
    }
}
