using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.IntegrationTest;

public class HeartBeatGeneratorShould
{
    private readonly IHeartBeatMonitor _fakeHeartBeatMonitor = Substitute.For<IHeartBeatMonitor>();
    private const double TestHeartBeatIntervalInMilliseconds = 1000 * 1;

    public HeartBeatGeneratorShould()
    {
        var _ = new HeartBeatGenerator(_fakeHeartBeatMonitor, TestHeartBeatIntervalInMilliseconds);
    }

    [Fact]
    public void SendOneHeartBeat()
    {
        Thread.Sleep((int)(TestHeartBeatIntervalInMilliseconds * 1.1));
        _fakeHeartBeatMonitor.Received(1).HeartBeat();
    }
}