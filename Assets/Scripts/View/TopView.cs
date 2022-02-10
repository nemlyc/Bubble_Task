using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TopView : MonoBehaviour
{
    [SerializeField]
    GameObject topViewRoot;
    [SerializeField]
    Button addButton;
    [SerializeField]
    GameObject bubblePrefab;

    [SerializeField]
    Transform bubbleRoot;

    [SerializeField]
    TaskViewModel taskViewModel;

    //public void RestoreBubbles()
    //{

    //}

    public void AddBubble(TaskEntity taskEntity)
    {
        if (taskEntity.IsCompleted)
        {
            return;
        }
        var bubbleObject = Instantiate(bubblePrefab, bubbleRoot);
        var bubble = bubbleObject.GetComponent<IBubble>();
        bubble.Initialize(taskEntity);

        bubbleObject.GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
        {
            taskViewModel.SetView(ViewState.Editor, bubble.GetID());
        }).AddTo(this);
    }

    public void UpdateTask(TaskEntity task)
    {
        var bubble = GetBubble(task.ID).GetComponent<IBubble>();
        bubble.UpdateInfo(task);
    }

    public void DeleteBubble(string ID)
    {
        var bubble = GetBubble(ID).gameObject;
        Destroy(bubble);
    }

    private void Start()
    {
        taskViewModel.ViewStatus.Subscribe(currentView =>
        {
            if (currentView.Equals(ViewState.Top))
            {
                topViewRoot.SetActive(true);
            }
            else
            {
                topViewRoot.SetActive(false);
            }
        }).AddTo(this);

        addButton.OnClickAsObservable().Subscribe(_ =>
        {
            taskViewModel.SetView(ViewState.Editor);
        }).AddTo(this);
    }

    private Transform GetBubble(string ID)
    {
        var s = bubbleRoot.Find(ID);
        return s;
    }
}
