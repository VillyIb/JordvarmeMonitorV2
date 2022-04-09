using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Util;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.UnitTests;

public class NotificationsShould
{
    private readonly IActivityNotifications _sut1;
    private readonly IHeartBeatNotifications _sut2;
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();

    public NotificationsShould()
    {
        var notifications = new Notifications(_emailSender);
        _sut1 = notifications;
        _sut2 = notifications;
    }

    // ReSharper disable StringLiteralTypo

    [Fact]
    public void SendNotifyStopped()
    {
        SystemDateTime.SetTime(new TimeSpan(12,13,00));
        _sut1.NotifyStopped();
        _emailSender.Received(1).Send(Arg.Is("Jordvarme styring stoppet 12:13"), Arg.Is("Jordvarme styring stoppet 12:13"));
    }

    [Fact]
    public void SendNotifyRunning()
    {
        SystemDateTime.SetTime(new TimeSpan(12, 13, 00));
        _sut1.NotifyRunning();
        _emailSender.Received(1).Send(Arg.Is("Jordvarme styring kører 12:13"), Arg.Is("Jordvarme styring kører 12:13"));
    }

    [Theory]
    [InlineData(0, 3, "Jordvarme styring stoppet 12:13", "Jordvarme styring har kl 12:13 været stoppet i 3 min.")]
    [InlineData(1, 0, "Jordvarme styring stoppet 12:13", "Jordvarme styring har kl 12:13 været stoppet i 1 time og 0 min.")]
    [InlineData(2, 22, "Jordvarme styring stoppet 12:13", "Jordvarme styring har kl 12:13 været stoppet i 2 timer og 22 min.")]
    public void SendHeartBeatStopped(int timer, int min, string expectedSubject, string expectedBody)
    {
        SystemDateTime.SetTime(new TimeSpan(12, 13, 00));
        _sut2.HeartBeatStopped(new TimeSpan(timer, min, 0));
        _emailSender.Received(1).Send(Arg.Is(expectedSubject), Arg.Is(expectedBody));
    }

    [Fact]
    public void SendHeartBeatOk()
    {
        SystemDateTime.SetTime(new TimeSpan(12, 13, 00));
        _sut2.HeartBeatOk();
        _emailSender.Received(1).Send(Arg.Is<string>("Jordvarme styring kører fortsat 12:13"), Arg.Is<string>("Jordvarme styring kører fortsat 12:13"));
    }

    // ReSharper restore StringLiteralTypo

}