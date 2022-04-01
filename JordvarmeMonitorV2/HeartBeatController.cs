using JordvarmeMonitorV2.Constants;

namespace JordvarmeMonitorV2;

public  class HeartBeatController : IHeartBeatController, IChangeMode
{
    private readonly IHeartBeatNotifications _notifications;
    private readonly DateTimeFacade _dateTimeFacade;

    public HeartBeatController(IHeartBeatNotifications notifications, DateTimeFacade dateTimeFacade)
    {
        _notifications = notifications;
        _dateTimeFacade = dateTimeFacade;
    }

    private bool _isRunningField;

    public bool IsRunning
    {
        get => _isRunningField;
        set
        {
            _isRunningField = value;
            if (!value)
            {
                LastHeartBeatSentOut = _dateTimeFacade.Now;
            }
        } }

    private DateTime? LastHeartBeatSentOut { get; set; }

    private bool ModeStoppedFilter()
    {
        if (LastHeartBeatSentOut is null) return true;

        if (LastHeartBeatSentOut.Value.AddHours(1) < _dateTimeFacade.Now) return true;

        return false;
    }

    private bool ModeRunningFilter()
    {
        if (_dateTimeFacade.Now < DateTime.Today.AddHours(6)) return false;

        if (LastHeartBeatSentOut is null) return true;

        return LastHeartBeatSentOut.Value.Date < DateTime.Today;
    }

    public void HeartBeat()
    {
        if (IsRunning)
        {
            if (!ModeRunningFilter()) { return; }
            LastHeartBeatSentOut = _dateTimeFacade.Now;
            _notifications.HeartBeatOk();
        }
        else
        {
            if (!ModeStoppedFilter()) { return; }
            LastHeartBeatSentOut = _dateTimeFacade.Now;
            _notifications.HeartBeatStopped();
        }
    }
}