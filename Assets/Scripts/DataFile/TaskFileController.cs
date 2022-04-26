using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TaskFileController
{
    readonly string TaskFileName = "BubbleTaskData.json";
    readonly string ErrorMsg = "-1";

    public void WriteData(ReactiveDictionary<string, TaskEntity> dict)
    {
        var json = JsonManager.GenerateJsonObject(dict);
        JsonManager.WriteJsonData(TaskFileName, json);
    }

    public ReactiveDictionary<string, TaskEntity> ReadData()
    {
        var json = JsonManager.ReadJsonData(TaskFileName);

        ReactiveDictionary<string, TaskEntity> dictionary;
        if (json.Equals(ErrorMsg) || json.Equals(""))
        {
            dictionary = new ReactiveDictionary<string, TaskEntity>();
        }
        else
        {
            dictionary = JsonManager.ExpandJsonData<ReactiveDictionary<string, TaskEntity>>(json);
            foreach (var item in dictionary.Keys)
            {
                dictionary[item].SetID(item);
            }
        }

        return dictionary;
    }
}
