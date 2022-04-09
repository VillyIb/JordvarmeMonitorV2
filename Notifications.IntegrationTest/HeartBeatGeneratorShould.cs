using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.IntegrationTest;

public class HeartBeatGeneratorShould
{
    private readonly IHeartBeatMonitor _fakeHeartBeatMonitor = Substitute.For<IHeartBeatMonitor>();

    public HeartBeatGeneratorShould()
    {
        var _ = new HeartBeatGenerator(_fakeHeartBeatMonitor);
    }

    [Fact]
    public void SendOneHeartBeat()
    {
        Thread.Sleep((int)(Settings.HeartBeatIntervalInMilliseconds * 1.1));
        _fakeHeartBeatMonitor.Received(1).HeartBeat();
    }
}