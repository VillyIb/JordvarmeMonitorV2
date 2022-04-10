using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2;

public class ActivityMonitor : IActivityMonitor
{
    private readonly IActivityNotifications _activityNotifications;
    private readonly IChangeMode? _modeTarget;

    private bool _isRunningField;

    public void Startup()
    {
        _activityNotifications.Startup(IsStartup);
    }

    public bool IsRunning
    {
        get => _isRunningField;
        private set
        {
            _isRunningField = value;
            if (_modeTarget is not null)
            {
                _modeTarget.IsRunning = value;
            }
        }
    }

    private bool IsStartup { get; set; }

    public ActivityMonitor(IActivityNotifications activityNotifications, IChangeMode? modeTarget)
    {
        _activityNotifications = activityNotifications;
        _modeTarget = modeTarget;
        IsStartup = true;
    }


    public void ActivityDetected()
    {
        if (!IsStartup && IsRunning) { return; }

        IsStartup = false;
        IsRunning = true;
        _activityNotifications.NotifyRunning();
    }

    public void TimeoutDetected()
    {
        if (!IsStartup && !IsRunning) { return; }

        IsStartup = false;
        IsRunning = false;
        _activityNotifications.NotifyStopped();
    }
}