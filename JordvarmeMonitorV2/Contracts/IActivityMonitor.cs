namespace JordvarmeMonitorV2.Contracts;

public interface IActivityMonitor
{
    void ActivityDetected();

    void TimeoutDetected();

    bool IsRunning { get; }
}