using System;
public class UserEntity
{
    public string ID { get; private set; }
    //public string Password { get; set; }
    //public string PasswordConfiguration { get; set; }

    public UserEntity()
    {
        ID = Guid.NewGuid().ToString("N");
    }

    public void SetID(string id)
    {
        this.ID = id;
    }
}
