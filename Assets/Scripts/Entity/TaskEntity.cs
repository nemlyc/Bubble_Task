using System;
public class TaskEntity
{
    public string ID { get; private set; }
    public string UserID { get; set; }
    public int Priority { get; set; }
    public string Desccription { get; set; }
    public string[] NodeIDs { get; set; }

    public TaskEntity()
    {
        ID = Guid.NewGuid().ToString("N");
    }

    public void SetID(string id)
    {
        this.ID = id;
    }
}

