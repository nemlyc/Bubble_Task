public interface IBubble
{
    void Initialize(TaskEntity task);
    void UpdateInfo(TaskEntity task);
    string GetID();
    void Crash();
    void Delete();
}
