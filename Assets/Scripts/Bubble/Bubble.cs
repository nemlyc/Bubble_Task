using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BubbleMovement))]
public class Bubble : MonoBehaviour, IBubble
{
    public string TaskID { get; private set; }

    [SerializeField]
    TMPro.TMP_Text bodyText;

    [SerializeField]
    Image backgroundImage, progressCircle;

    [SerializeField]
    Sprite[] bubbleColors;

    InputDetector input;

    public void Crash()
    {
        var taskViewModel = FindObjectOfType<TaskViewModel>();
        taskViewModel.CompleteTask(TaskID);

        Destroy(gameObject);
    }

    public void Initialize(TaskEntity task)
    {
        TaskID = task.ID;
        gameObject.name = TaskID;

        var colorIndex = Random.Range(0, bubbleColors.Length);
        backgroundImage.sprite = bubbleColors[colorIndex];

        GetComponent<BubbleMovement>().InitializePosition();
        UpdateInfo(task);

        input = gameObject.AddComponent<InputDetector>();
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

    void Update()
    {
        if (input.isTimerStarted)
        {
            progressCircle.fillAmount = input.CurrentHoldingNormalTime();
        }
        else
        {
            progressCircle.fillAmount = 0;
        }
    }
}
