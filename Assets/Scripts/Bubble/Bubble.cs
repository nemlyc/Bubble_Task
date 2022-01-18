using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour, IBubble
{
    public string TaskID { get; private set; }

    [SerializeField]
    TMPro.TMP_Text bodyText;

    public void Crash(string crashID)
    {

    }

    public void Initialize(TaskEntity task)
    {
        TaskID = task.ID;
        gameObject.name = TaskID;

        UpdateInfo(task);
    }

    public void UpdateInfo(TaskEntity task)
    {
        var scale = new BubbleScaler().GetScale(task.Priority);
        transform.localScale = scale;
        bodyText.text = task.Description;
    }

    public void Delete()
    {
        
    }

    public string GetID()
    {
        return TaskID;
    }
}
