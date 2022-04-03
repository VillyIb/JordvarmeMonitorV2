using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Util;

namespace JordvarmeMonitorV2;

public  class HeartBeatController : IHeartBeatController, IChangeMode
{
    private readonly IHeartBeatNotifications _notifications;

    public HeartBeatController(IHeartBeatNotifications notifications)
    {
        _notifications = notifications;
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
                LastHeartBeatSentOut = SystemDateTime.Now;
            }
        } }

    private DateTime? LastHeartBeatSentOut { get; set; }

    private bool ModeStoppedFilter()
    {
        if (LastHeartBeatSentOut is null) return true;

        if (LastHeartBeatSentOut.Value.AddHours(1) <= SystemDateTime.Now) return true;

        return false;
    }

    private bool ModeRunningFilter()
    {
        if (SystemDateTime.Now < SystemDateTime.Today.AddHours(6)) return false;

        if (LastHeartBeatSentOut is null) return true;

        return 
            (LastHeartBeatSentOut.Value < SystemDateTime.Today.AddHours(6 ))
               && 
               (LastHeartBeatSentOut.Value.Date <= SystemDateTime.Today)
        ;
    }

    public void HeartBeat()
    {
        if (IsRunning)
        {
            if (!ModeRunningFilter()) { return; }
            LastHeartBeatSentOut = SystemDateTime.Now;
            _notifications.HeartBeatOk();
        }
        else
        {
            if (!ModeStoppedFilter()) { return; }
            LastHeartBeatSentOut = SystemDateTime.Now;
            _notifications.HeartBeatStopped();
        }
    }
}