using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Tx;

namespace JordvarmeMonitorV2;

public  class Notifications : INotifications
{
    private static readonly string StartMessage = @"Jordvarme styring monitor startet";
    private static readonly string ReadyMessage = @"Ready";
    private static readonly string RunningMessage = @"Jordvarme styring kører";
    private static readonly string StoppedMessage = @"Jordvarme styring: stoppet {0:HH:mm}";
    private static readonly string StillStoppedMessage = @"Jordvarme styring: fortsat stoppet";
    private static readonly string DailyMessage = @"Jordvarme styring: kører fortsat";

    private EmailSender emailSender;

    public Notifications()
    {
        emailSender = new EmailSender();
    }


    public void NotifyRunning()
    {
        emailSender.Send(RunningMessage);
    }

    public void NotifyStopped(TimeSpan duration)
    {
        emailSender.Send(string.Format(StoppedMessage,duration));
    }

    public void NotifyStopped()
    {
        emailSender.Send(StoppedMessage);
    }


    public void HeartBeatOk()
    {
        emailSender.Send(DailyMessage);
    }

    public void HeartBeatStopped()
    {
        emailSender.Send(StoppedMessage);
    }
}