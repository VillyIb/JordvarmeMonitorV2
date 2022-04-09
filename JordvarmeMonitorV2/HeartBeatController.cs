﻿using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Util;

namespace JordvarmeMonitorV2;

public class HeartBeatController : IHeartBeatController, IChangeMode
{
#if DEBUG
    public static readonly TimeSpan DurationBetweenHeartBeatStopped = new TimeSpan(0, 5, 0);
#else
    public static readonly TimeSpan DurationBetweenHeartBeatStopped = new TimeSpan(1, 0, 0);
#endif

    private readonly IHeartBeatNotifications _notifications;

    public HeartBeatController(IHeartBeatNotifications notifications)
    {
        _notifications = notifications;
    }

    private bool _isRunningField;

    private DateTime? LastHeartBeatSentOut { get; set; }

    private DateTime? TimeOfFailure { get; set; }

    public bool IsRunning
    {
        get => _isRunningField;
        set
        {
            _isRunningField = value;
            LastHeartBeatSentOut = SystemDateTime.Now;

            if (value) return;
            TimeOfFailure = SystemDateTime.Now;
        }
    }


    private bool ModeStoppedFilter()
    {
        if (LastHeartBeatSentOut is null)
        {
            return true;
        }

        if (LastHeartBeatSentOut.Value.Add(DurationBetweenHeartBeatStopped) <= SystemDateTime.Now)
        {
            return true;
        }

        return false;
    }

    private bool ModeRunningFilter()
    {
        if (SystemDateTime.Now < SystemDateTime.Today.AddHours(6)) return false;

        if (LastHeartBeatSentOut is null) return true;

        return
            LastHeartBeatSentOut.Value < SystemDateTime.Today.AddHours(6)
               &&
               LastHeartBeatSentOut.Value.Date <= SystemDateTime.Today
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

            var duration =
                SystemDateTime.Now.Subtract(TimeOfFailure ?? SystemDateTime.Now);
            LastHeartBeatSentOut = SystemDateTime.Now;
            _notifications.HeartBeatStopped(duration);
        }
    }
}