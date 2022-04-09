namespace JordvarmeMonitorV2.Contracts;

public interface INotifications
{
    void NotifyRunning();

    void NotifyStopped();
}