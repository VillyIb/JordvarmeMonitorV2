namespace JordvarmeMonitorV2.Contracts;

public interface IActivityNotifications
{
    void NotifyRunning();

    void NotifyStopped();

    void Startup(bool isStartup);
}