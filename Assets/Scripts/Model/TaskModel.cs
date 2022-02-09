using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class TaskModel
{
    public ReactiveDictionary<string, TaskEntity> taskDictionary;
    Subject<TaskEntity> updateTask = new Subject<TaskEntity>();
    Subject<TaskEntity> completeTask = new Subject<TaskEntity>();
    
    public IObservable<DictionaryAddEvent<string, TaskEntity>> AddDictionary => taskDictionary.ObserveAdd();
    public IObservable<DictionaryRemoveEvent<string, TaskEntity>> RemoveDictionary => taskDictionary.ObserveRemove();
    public IObservable<TaskEntity> UpdateTaskInfo => updateTask.AsObservable();
    public IObservable<TaskEntity> CompleteTaskObserbable => completeTask.AsObservable();

    TaskFileController fileManager;

    public TaskModel()
    {
        taskDictionary = new ReactiveDictionary<string, TaskEntity>();
        fileManager = new TaskFileController();
    }

    public TaskEntity CreateTask(UserEntity userID, int priority, string description, List<string> node)
    {
        TaskEntity entity = new TaskEntity
        {
            UserID = userID.ID,
            IsCompleted = false,
            Priority = priority,
            Description = description,
            NodeIDs = node
        };
        taskDictionary.Add(entity.ID, entity);

        fileManager.WriteData(taskDictionary);

        return entity;
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

    public ReactiveDictionary<string, TaskEntity> ReadTaskDictionary()
    {
        taskDictionary = fileManager.ReadData();
        return taskDictionary;
    }
    public TaskEntity ReadTask(string ID)
    {
        var gotValue = taskDictionary.TryGetValue(ID, out TaskEntity value);
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
        updateTask.OnNext(entity);
    }

    public void UpdatePriority(string ID, int priority)
    {
        ReadTask(ID).Priority = priority;
        updateTask.OnNext(ReadTask(ID));
    }

    public void UpdateDescription(string ID, string description)
    {
        ReadTask(ID).Description = description;
        updateTask.OnNext(ReadTask(ID));
    }

    public void UpdateNodeIDs(string ID, List<string> node)
    {
        ReadTask(ID).NodeIDs = node;
        updateTask.OnNext(ReadTask(ID));
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
                //updateTask.OnNext(ReadTask(ID));
                completeTask.OnNext(ReadTask(ID));
            }
        }
        else
        {
            targetEntity.IsCompleted = true;
            //updateTask.OnNext(ReadTask(ID));
            completeTask.OnNext(ReadTask(ID));
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
