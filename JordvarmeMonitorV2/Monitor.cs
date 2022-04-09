using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2;

public class Monitor : IFileSystemWatcherClient
{
    private readonly INotifications _notifications;
    private readonly IChangeMode? _modeTarget;

    private bool _isRunningField;

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
    public bool IsStartup { get; protected set; }

    public Monitor(INotifications notifications, IChangeMode? modeTarget)
    {
        _notifications = notifications;
        _modeTarget = modeTarget;
        IsStartup = true;
    }


    public void ActivityDetected()
    {
        if (!IsStartup && IsRunning) { return; }

        IsStartup = false;
        IsRunning = true;
        _notifications.NotifyRunning();
    }

    public void TimeoutDetected()
    {
        if (!IsStartup && !IsRunning) { return; }

        IsStartup = false;
        IsRunning = false;
        _notifications.NotifyStopped();
    }
}