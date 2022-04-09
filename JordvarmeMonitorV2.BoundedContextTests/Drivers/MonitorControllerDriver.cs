using JordvarmeMonitorV2.BoundedContextTests.Support;
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2.BoundedContextTests.Drivers;

public class MonitorControllerDriver
{
    private readonly IActivityMonitor _activityMonitor;

    public MonitorControllerDriver(
        IActivityNotifications activityNotifications
    )
    {
        _activityMonitor = new ActivityMonitor(activityNotifications, null);
    }

    public void StubEventReceived(Event @event)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (@event)
        {
            case Event.Timeout:
            {
                _activityMonitor.TimeoutDetected();
                break;
            }
            case Event.Update:
            {
                _activityMonitor.ActivityDetected();
                break;
            }
        }
    }

    public Mode GetMode()
    {
        return _activityMonitor.IsRunning ? Mode.Running : Mode.Stopped;
    }
}