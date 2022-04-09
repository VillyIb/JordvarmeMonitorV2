namespace JordvarmeMonitorV2.Contracts;

public interface IFileSystemWatcherClient
{
    void ActivityDetected();

    void TimeoutDetected();

    bool IsRunning { get; }

    bool IsStartup { get; }
}