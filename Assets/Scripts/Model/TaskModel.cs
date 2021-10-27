using System;
using System.Collections.Generic;

public class TaskModel
{
    private Dictionary<string, TaskEntity> taskDictionary;

    public TaskModel()
    {
        taskDictionary = new Dictionary<string, TaskEntity>();
    }

    public void CreateTask(UserEntity userID, int priority, string description, List<string> node)
    {
        TaskEntity entity = new TaskEntity
        {
            UserID = userID.ID,
            IsCompleted = false,
            Priority = priority,
            Desccription = description,
            NodeIDs = node
        };

        taskDictionary.Add(entity.ID, entity);
    }

    public void AddSubTask(string parentID, string childID)
    {
        var parent = ReadTask(parentID);
        if (parent.NodeIDs == null)
        {
            parent.NodeIDs = new List<string>();
        }
        parent.NodeIDs.Add(childID);
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

    public void UpdateNodeIDs(string ID, List<string> node)
    {
        ReadTask(ID).NodeIDs = node;
    }

    public void CompleteTask(string ID)
    {
        /*
         * �q�m�[�h����������Ă��邩�m�F�B
         * �q�m�[�h����������Ă��Ȃ��ꍇ�F���ׂď���
         *                               �������Ȃ�
         */

        TaskEntity targetEntity = ReadTask(ID);

        if (targetEntity.NodeIDs != null)
        {
            var isCompleted = CheckSubNodeCompleted(targetEntity);
            if (!isCompleted)
            {
                //�I����Ă��Ȃ����B
            }
            else
            {
                targetEntity.IsCompleted = true;
            }
        }
        else
        {
            targetEntity.IsCompleted = true;
        }
    }

    public void DeleteTask(string key)
    {
        taskDictionary.Remove(key);
    }

    /// <summary>
    /// �T�u�m�[�g�^�X�N���������Ă��邩�m���߂�B
    /// </summary>
    /// <param name="parentTask">�e�^�X�N</param>
    /// <returns></returns>
    bool CheckSubNodeCompleted(TaskEntity parentTask)
    {
        bool isSubNodeCompleted = false;
        foreach (var item in parentTask.NodeIDs)
        {
            // �T�u�^�X�N���������Ă��鎞true
            if (ReadTask(item).IsCompleted)
            {
                isSubNodeCompleted = true;
            }
        }
        return isSubNodeCompleted;
    }
}
