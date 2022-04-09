using System;
using JordvarmeMonitorV2.BoundedContextTests.Support;
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2.BoundedContextTests.Drivers;

public class NotifyAliveDriver
{
    private readonly IHeartBeatMonitor _monitorClient;
    private readonly IActivityMonitor _activityMonitor;
    private readonly TimeSpan _fiveMinutes = new (0,5,0);

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

    public NotifyAliveDriver(
        IActivityNotifications activityNotifications,
        IHeartBeatNotifications heartBeatNotifications
    )
    {
        _monitorClient = new HeartBeatMonitor(heartBeatNotifications, _fiveMinutes);
        _activityMonitor = new ActivityMonitor(activityNotifications, (IChangeMode)_monitorClient);
    }

    public void StubNotifyEvent()
    {
        _monitorClient.HeartBeat();
    }
}