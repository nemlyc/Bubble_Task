using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TaskFileController : MonoBehaviour
{
    readonly string TaskFileName = "BubbleTaskData.json";

    public void WriteData(ReactiveDictionary<string, TaskEntity> dict)
    {
        var json = JsonManager.GenerateJsonObject(dict);
        JsonManager.WriteJsonData(TaskFileName, json);
    }

    public ReactiveDictionary<string, TaskEntity> ReadData()
    {
        var json = JsonManager.ReadJsonData(TaskFileName);
        var dict = JsonManager.ExpandJsonData<ReactiveDictionary<string, TaskEntity>>(json);
        return dict;
    }
}
