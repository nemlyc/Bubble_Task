public interface IBubble
{
    void Initialize(TaskEntity task);
    void UpdateInfo(TaskEntity task);
    void Crash(string crashID);
    void Delete();
}
