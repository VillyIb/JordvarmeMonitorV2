namespace JordvarmeMonitorV2.Contracts;

public interface IHeartBeatNotifications
{
    void HeartBeatOk();

    void HeartBeatStopped(TimeSpan duration);
}