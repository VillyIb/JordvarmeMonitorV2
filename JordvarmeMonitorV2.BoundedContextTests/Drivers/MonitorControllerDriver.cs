using JordvarmeMonitorV2.BoundedContextTests.Support;
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2.BoundedContextTests.Drivers;

public class MonitorControllerDriver
{
    private readonly IFileSystemWatcherClient _fileSystemWatcherClient;

    public MonitorControllerDriver(
        INotifications notifications
    )
    {
        _fileSystemWatcherClient = new Monitor(notifications, null);
    }

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

    public Mode GetMode()
    {
        return _fileSystemWatcherClient.IsRunning ? Mode.Running : Mode.Stopped;
    }
}