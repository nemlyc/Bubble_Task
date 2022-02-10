using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class TaskModelTest
{
    UserEntity user = new UserEntity();

    [Test]
    public void CreateTask()
    {
        var model = CreateModel(user);

        var dic = model.ReadTaskDictionary();
        var keys = dic.Keys;

        TaskEntity entity = new TaskEntity();

        foreach (var item in keys)
        {
            entity = dic[item];
        }

        Assert.That(model.ReadTaskDictionary().Count > 0);
    }

    [Test]
    public void CreateSubTask()
    {
        var model = CreateModel(user);
        
        model.CreateTask(user, 1, "追加したタスク", null);

        List<string> ids = CreateTaskIDList(model);

        Assert.IsNull(model.ReadTask(ids[0]).NodeIDs);

        model.AddSubTask(ids[0], ids.Last());

        Assert.IsNotNull(model.ReadTask(ids[0]).NodeIDs);
    }

    [Test]
    public void ReadTask()
    {
        var model = CreateModel(user);

        List<string> ids = CreateTaskIDList(model);

        foreach (var item in ids)
        {
            var ent = model.ReadTask(item);
            Assert.NotNull(ent);
            CheckTaskValues(ent);
        }
    }

    [Test]
    public void UpdatePriority()
    {
        /*
         * 更新前と後で値が変わっているかを確かめる。
         */

        var model = CreateModel(user);
        List<string> ids = CreateTaskIDList(model);
        List<int> before = new List<int>();

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            before.Add(entity.Priority);
            model.UpdatePriority(ids[i], 4);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            var beforeVal = before[i];
            int afterVal = entity.Priority;
            Assert.AreNotEqual(beforeVal, afterVal);
        }
    }
    [Test]
    public void UpdateDescription()
    {
        var model = CreateModel(user);
        List<string> ids = CreateTaskIDList(model);
        List<string> before = new List<string>();

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            before.Add(entity.Description);
            model.UpdateDescription(ids[i], "updated");
        }

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            var beforeVal = before[i];
            string afterVal = entity.Description;
            Assert.AreNotEqual(beforeVal, afterVal);
        }
    }
    [Test]
    public void UpdateNodeIDs()
    {
        var model = CreateModel(user);
        List<string> ids = CreateTaskIDList(model);
        List<List<string>> before = new List<List<string>>();

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            before.Add(entity.NodeIDs);
            List<string> node = new List<string>();
            node.Add("subtask_1");
            node.Add("subtask_2");
            node.Add("subtask_3");
            node.Add("subtask_4");
            model.UpdateNodeIDs(ids[i], node);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            var beforeVal = before[i];
            List<string> afterVal = entity.NodeIDs;
            Assert.AreNotEqual(beforeVal, afterVal);
        }
    }

    [Test]
    public void CompleteTaskTest()
    {
        UserEntity ue = new UserEntity();

        TaskModel model = new TaskModel();
        model.CreateTask(ue, 1, "だいじ_1", null);
        model.CreateTask(ue, 2, "だいじ_2", null);

        List<string> ids = CreateTaskIDList(model);

        foreach (var item in ids)
        {
            var ent = model.ReadTask(item);
            Assert.AreEqual(ent.IsCompleted, false);
            model.CompleteTask(item);
            Assert.AreEqual(ent.IsCompleted, true);
        }
    }

    void CheckTaskValues(TaskEntity entity)
    {
        Assert.IsNotNull(entity.ID);
        Assert.IsNotNull(entity.UserID);
        Assert.IsNotNull(entity.IsCompleted);
        Assert.IsNotNull(entity.Priority);
        Assert.IsNotNull(entity.Description);
        if (entity.NodeIDs != null)
        {
            Assert.IsNotNull(entity.NodeIDs.Count > 0);
        }
    }

    TaskModel CreateModel(UserEntity ue)
    {
        TaskModel model = new TaskModel();
        model.CreateTask(ue, 1, "だいじ_1", null);                     // 0
        model.CreateTask(ue, 2, "だいじ_2", null);                     // 1
        model.CreateTask(ue, 1, "サブタスクあり", null);               // 2

        model.CreateTask(ue, 1, "サブタスク1", null);                  // 3
        model.CreateTask(ue, 1, "サブタスク2 - サブタスクあり", null);  // 4

        model.CreateTask(ue, 2, "サブタスクのサブタスク", null);        // 5
        
        List<string> ids = CreateTaskIDList(model);

        model.AddSubTask(ids[2], ids[3]);
        model.AddSubTask(ids[2], ids[4]);

        model.AddSubTask(ids[4], ids[5]);

        return model;
    }

    List<string> CreateTaskIDList(TaskModel model)
    {
        List<string> taskIDList = new List<string>();
        foreach (var item in model.ReadTaskDictionary().Keys)
        {
            taskIDList.Add(item);
        }
        return taskIDList;
    }
}
