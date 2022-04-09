using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Util;

namespace JordvarmeMonitorV2;

public  class Notifications : INotifications, IHeartBeatNotifications
{
    // ReSharper disable StringLiteralTypo

    private const string RunningSubject = @"Jordvarme styring kører {0:HH\:mm}";
    private const string RunningBody = @"Jordvarme styring kører {0:HH\:mm}";
            
    private const string StoppedSubject = @"Jordvarme styring stoppet {0:HH\:mm}";
    private const string StoppedBodyUpToOneHour = @"Jordvarme styring har kl {0:HH\:mm} været stoppet i {1:%m} min.";
    private const string StoppedBodyOneToTwoHours = @"Jordvarme styring har kl {0:HH\:mm} været stoppet i {1:%h} time og {1:%m} min.";
    private const string StoppedBodyTwoHoursPlus = @"Jordvarme styring har kl {0:HH\:mm} været stoppet i {1:%h} timer og {1:%m} min.";
    
    private const string DailyMessage = @"Jordvarme styring kører fortsat {0:HH\:mm}";

    // ReSharper restore StringLiteralTypo

    private readonly IEmailSender _emailSender;

    public Notifications(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public void NotifyRunning()
    {
        var subject = string.Format(RunningSubject, SystemDateTime.Now);
        var body = string.Format(RunningBody, SystemDateTime.Now);
        _emailSender.Send(subject, body);
    }

    public void NotifyStopped()
    {
        var subject = string.Format(StoppedSubject, SystemDateTime.Now);
        _emailSender.Send(subject, subject);
    }
    
    public void HeartBeatOk()
    {
        var subject = string.Format(DailyMessage, SystemDateTime.Now);
        _emailSender.Send(subject, subject);
    }

    public void HeartBeatStopped(TimeSpan duration)
    {
        string format;
        var oneHour = new TimeSpan(1, 0, 0);
        var twoHours = new TimeSpan(2, 0, 0);

        if (duration < oneHour)
        {
            format = StoppedBodyUpToOneHour;
        }
        else if (oneHour <= duration && duration < twoHours)
        {
            format = StoppedBodyOneToTwoHours;
        }
        else
        {
            format = StoppedBodyTwoHoursPlus;
        }

        var subject = string.Format(StoppedSubject, SystemDateTime.Now);
        var body = string.Format(format, SystemDateTime.Now, duration);
        _emailSender.Send(subject, body);
    }
}