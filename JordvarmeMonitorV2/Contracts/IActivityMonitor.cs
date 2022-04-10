namespace JordvarmeMonitorV2.Contracts;

public interface IActivityMonitor
{
    void ActivityDetected();

    void TimeoutDetected();

    void Startup();

    bool IsRunning { get; }
}