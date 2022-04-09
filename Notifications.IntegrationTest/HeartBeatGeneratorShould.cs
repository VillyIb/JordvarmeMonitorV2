using System.Transactions;
using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.IntegrationTest;

public class HeartBeatGeneratorShould
{
    private readonly IHeartBeatController _fakeHeartBeatController = Substitute.For<IHeartBeatController>();

    public HeartBeatGeneratorShould()
    {
        var _ = new HeartBeatGenerator(_fakeHeartBeatController);
    }

    [Fact]
    public void SendOneHeartBeat()
    {
        Thread.Sleep((int)(HeartBeatGenerator.IntervalInMilliseconds * 1.1));
        _fakeHeartBeatController.Received(1).HeartBeat();
    }
}