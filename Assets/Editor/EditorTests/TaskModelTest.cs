using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TaskModelTest
{
    [Test]
    public void CreateTask()
    {
        var model = CreateModel();

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
    public void ReadTask()
    {
        var model = CreateModel();

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

        var model = CreateModel();
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
        var model = CreateModel();
        List<string> ids = CreateTaskIDList(model);
        List<string> before = new List<string>();

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            before.Add(entity.Desccription);
            model.UpdateDescription(ids[i], "updated");
        }

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            var beforeVal = before[i];
            string afterVal = entity.Desccription;
            Assert.AreNotEqual(beforeVal, afterVal);
        }
    }
    [Test]
    public void UpdateNodeIDs()
    {
        var model = CreateModel();
        List<string> ids = CreateTaskIDList(model);
        List<string[]> before = new List<string[]>();

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            before.Add(entity.NodeIDs);
            string[] node = new string[4];
            node[0] = "subtask_1";
            node[1] = "subtask_2";
            node[2] = "subtask_3";
            node[3] = "subtask_4";
            model.UpdateNodeIDs(ids[i], node);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            TaskEntity entity = model.ReadTask(ids[i]);
            var beforeVal = before[i];
            string[] afterVal = entity.NodeIDs;
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
        Assert.IsNotNull(entity.Desccription);
        if (entity.NodeIDs != null)
        {
            Assert.IsNotNull(entity.NodeIDs.Length > 0);
        }
    }

    TaskModel CreateModel()
    {
        UserEntity ue = new UserEntity();

        TaskModel model = new TaskModel();
        model.CreateTask(ue, 1, "だいじ_1", null);
        model.CreateTask(ue, 2, "だいじ_2", null);

        string[] node = new string[2];
        node[0] = "subtask_1";
        node[1] = "subtask_2";

        model.CreateTask(ue, 3, "だいじ_3", node);

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
