namespace UsedParts.PhoneServices
{
    public interface ITaskAgentManager
    {
        string Name { get; }
        bool IsRunning { get; }
        bool CanRun { get; }

        void Run();
        void Stop();
    }
}
