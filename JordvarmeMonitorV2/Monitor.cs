using JordvarmeMonitorV2.Constants;

namespace JordvarmeMonitorV2;

public  class Monitor : IFileSystemWatcherClient
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
            if(_modeTarget is not null) {_modeTarget.IsRunning = value;}
        }
    }

    public Monitor(INotifications notifications, IChangeMode? modeTarget)
    {
        _notifications = notifications;
        _modeTarget = modeTarget;
    }

    public void ActivityDetected()
    {
        if(IsRunning) {return;}

        IsRunning = true;
        _notifications.NotifyRunning();
    }

    public void TimeoutDetected()
    {
        if(!IsRunning) {return;}
        IsRunning = false;
        _notifications.NotifyStopped();
    }
}