using JordvarmeMonitorV2.BoundedContextTests.Support;
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2.BoundedContextTests.Drivers;

public class NotifyAliveDriver
{
    private readonly IHeartBeatController _monitorClient;
    private readonly IFileSystemWatcherClient _fileSystemWatcherClient;

    public void StubEventReceived(Event @event)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (@event)
        {
            case Event.Timeout:
            {
                _fileSystemWatcherClient.TimeoutDetected();
                break;
            }
            case Event.Update:
            {
                _fileSystemWatcherClient.ActivityDetected();
                break;
            }
        }
    }

    public NotifyAliveDriver(
        INotifications notifications,
        IHeartBeatNotifications heartBeatNotifications
    )
    {
        _monitorClient = new HeartBeatController(heartBeatNotifications);
        _fileSystemWatcherClient = new Monitor(notifications, (IChangeMode)_monitorClient);
    }

    public void StubNotifyEvent()
    {
        _monitorClient.HeartBeat();
    }
}