using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Util;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.UnitTests;

public class HeartBeatMonitorShould
{
    private readonly HeartBeatMonitor _sut;
    private readonly IHeartBeatNotifications _fakeNotifications;
    private readonly TimeSpan _durationBetweenHeartBeatStopped = new(0, 5, 0);

    public HeartBeatMonitorShould()
    {
        _fakeNotifications = Substitute.For<IHeartBeatNotifications>();
        _sut = new HeartBeatMonitor(_fakeNotifications, _durationBetweenHeartBeatStopped);
    }

    [Fact]
    public void NotReceiveHeartBeatStopped()
    {
        SystemDateTime.SetTime(new TimeSpan(0, 0, 0));
        _sut.IsRunning = false;
        _sut.HeartBeat();
        _fakeNotifications.Received(0).HeartBeatStopped(Arg.Any<TimeSpan>());
    }

    [Fact]
    public void ReceiveOneHeartBeatStopped()
    {
        SystemDateTime.SetTime(new TimeSpan(0, 0, 0));
        _sut.IsRunning = false;
        _sut.HeartBeat();
        SystemDateTime.SetTime(_durationBetweenHeartBeatStopped);
        _sut.HeartBeat();
        _sut.HeartBeat();
        _fakeNotifications.Received(1).HeartBeatStopped(Arg.Is(_durationBetweenHeartBeatStopped));
    }

    [Fact]
    public void ReceiveTwoHeartBeatStopped()
    {
        SystemDateTime.SetTime(new TimeSpan(0, 0, 0));
        _sut.IsRunning = false;
        _sut.HeartBeat();
        SystemDateTime.SetTime(_durationBetweenHeartBeatStopped);
        _sut.HeartBeat();
        SystemDateTime.SetTime(_durationBetweenHeartBeatStopped*2);
        _sut.HeartBeat();
        _fakeNotifications.Received(1).HeartBeatStopped(Arg.Is(_durationBetweenHeartBeatStopped));
        _fakeNotifications.Received(1).HeartBeatStopped(Arg.Is(_durationBetweenHeartBeatStopped*2));
    }

    [Fact]
    public void NotReceiveHeartBeatOk()
    {
        SystemDateTime.SetTime(new TimeSpan(0, 0, 0));
        _sut.IsRunning = true;
        _sut.HeartBeat();
        _fakeNotifications.Received(0).HeartBeatOk();
    }

    [Fact]
    public void Receive1HeartBeatOk()
    {
        SystemDateTime.SetTime(new TimeSpan(0, 0, 0));
        _sut.IsRunning = true;
        _sut.HeartBeat();
        SystemDateTime.SetTime(new TimeSpan(6, 0, 0));
        _sut.HeartBeat();
        _sut.HeartBeat();
        _fakeNotifications.Received(1).HeartBeatOk();
    }
    [Fact]
    public void Receive2HeartBeatOk()
    {
        SystemDateTime.SetTime(new TimeSpan(0, 0, 0));
        _sut.IsRunning = true;
        _sut.HeartBeat();
        SystemDateTime.SetTime(new TimeSpan(6, 0, 0));
        _sut.HeartBeat();
        SystemDateTime.SetTime(new TimeSpan(1, 6, 0, 0));
        _sut.HeartBeat();
        _fakeNotifications.Received(2).HeartBeatOk();
    }

    [Fact]
    public void ReceiveHeartBeatStoppedHavingDefaultIsRunning()
    {
        _sut.HeartBeat();
        _fakeNotifications.Received(1).HeartBeatStopped(Arg.Any<TimeSpan>());
    }
}