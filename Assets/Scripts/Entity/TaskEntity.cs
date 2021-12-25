using System;
using System.Collections.Generic;

public class TaskEntity
{
    public string ID { get; private set; }
    public string UserID { get; set; }
    public bool IsCompleted { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }
    public List<string> NodeIDs { get; set; }

    public TaskEntity()
    {
        ID = Guid.NewGuid().ToString("N");
    }

    public void SetID(string id)
    {
        this.ID = id;
    }
}

