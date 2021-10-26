using System;
using System.Collections.Generic;

public class TaskModel
{
    private Dictionary<string, TaskEntity> taskDictionary;

    public TaskModel()
    {
        taskDictionary = new Dictionary<string, TaskEntity>();
    }

    public void CreateTask(UserEntity userID, int priority, string description, string[] node)
    {
        TaskEntity entity = new TaskEntity
        {
            UserID = userID.ID,
            Priority = priority,
            Desccription = description,
            NodeIDs = node
        };

        taskDictionary.Add(entity.ID, entity);
    }

    public Dictionary<string, TaskEntity> ReadTaskDictionary()
    {
        return taskDictionary;
    }
    public TaskEntity ReadTask(string key)
    {
        var gotValue = taskDictionary.TryGetValue(key, out TaskEntity value);
        if (!gotValue)
        {
            return null;
        }
        return value;
    }

    public void UpdateTask(TaskEntity entity)
    {
        var key = entity.ID;
        taskDictionary[key] = entity;
    }

    public void UpdatePriority(string ID, int priority)
    {
        ReadTask(ID).Priority = priority;
    }

    public void UpdateDescription(string ID, string description)
    {
        ReadTask(ID).Desccription = description;
    }

    public void UpdateNodeIDs(string ID, string[] node)
    {
        ReadTask(ID).NodeIDs = node;
    }

    public void DeleteTask(string key)
    {
        taskDictionary.Remove(key);
    }
}
