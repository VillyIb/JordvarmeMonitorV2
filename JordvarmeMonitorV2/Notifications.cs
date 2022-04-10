using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Util;

namespace JordvarmeMonitorV2;

public class Notifications : IActivityNotifications, IHeartBeatNotifications
{
    private readonly IEmailSender _emailSender;

    public Notifications(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public void NotifyRunning()
    {
        var subject = string.Format(Text.RunningSubject, SystemDateTime.Now);
        var body = string.Format(Text.RunningBody, SystemDateTime.Now);
        _emailSender.Send(subject, body);
    }

    public void NotifyStopped()
    {
        var subject = string.Format(Text.StoppedSubject, SystemDateTime.Now);
        _emailSender.Send(subject, subject);
    }

    public void Startup(bool isStartup)
    {
        var subject = string.Format(Text.StartupSubject, SystemDateTime.Now);
        var body = string.Format(isStartup ? Text.StartupBodyStartup : Text.StartupBodyRestart, SystemDateTime.Now);
        _emailSender.Send(subject, body);
    }

    public void HeartBeatOk()
    {
        var subject = string.Format(Text.DailyMessage, SystemDateTime.Now);
        _emailSender.Send(subject, subject);
    }

    public void HeartBeatStopped(TimeSpan duration)
    {
        string format;
        var oneHour = new TimeSpan(1, 0, 0);
        var twoHours = new TimeSpan(2, 0, 0);

        if (duration < oneHour)
        {
            format = Text.StoppedBodyUpToOneHour;
        }
        else if (oneHour <= duration && duration < twoHours)
        {
            format = Text.StoppedBodyOneToTwoHours;
        }
        else
        {
            format = Text.StoppedBodyTwoHoursPlus;
        }

        var subject = string.Format(Text.StoppedSubject, SystemDateTime.Now);
        var body = string.Format(format, SystemDateTime.Now, duration);
        _emailSender.Send(subject, body);
    }
}