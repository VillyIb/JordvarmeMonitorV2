using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.UnitTests;

public class ActivityMonitorShould
{
    private readonly ActivityMonitor _sut;
    private readonly IActivityNotifications _fakeActivityNotifications = Substitute.For<IActivityNotifications>();

    public ActivityMonitorShould()
    {
        _sut = new ActivityMonitor(_fakeActivityNotifications, null);
    }

    [Fact]
    public void UnconditionallySendRunningUponFirstActivityDetected()
    {
        _sut.ActivityDetected();
        _fakeActivityNotifications.Received(1).NotifyRunning();
    }

    [Fact]
    public void OnlySendOneRunningUponMultipleActivityDetected()
    {
        _sut.ActivityDetected();
        _sut.ActivityDetected();
        _sut.ActivityDetected();
        _fakeActivityNotifications.Received(1).NotifyRunning();
    }

    [Fact]
    public void UnconditionallySendStoppedUponFirstTimeoutDetected()
    {
        _sut.TimeoutDetected();
        _fakeActivityNotifications.Received(1).NotifyStopped();
    }

    [Fact]
    public void OnlySendOneStoppedUponMultipleActivityDetected()
    {
        _sut.TimeoutDetected();
        _sut.TimeoutDetected();
        _sut.TimeoutDetected();
        _fakeActivityNotifications.Received(1).NotifyStopped();
    }
}