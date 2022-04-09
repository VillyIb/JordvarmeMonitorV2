using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;
// ReSharper disable StringLiteralTypo

namespace JordvarmeMonitorV2.IntegrationTest;

[TestCaseOrderer("Alfa", "Bravo")]
public class FileAndTimerFacadeShould
{
    private readonly IActivityMonitor _fakeClient = Substitute.For<IActivityMonitor>();

    private double _activityTimeoutIntervalInMilliseconds = 1000 * 2;


    private static void UpdateDirectory()
    {
        var fileInfo = new FileInfo(Path.Combine(Settings.WatchPath, "testfile.log"));
        using var sw = fileInfo.CreateText();
        sw.Flush();
        sw.Close();
    }

    public FileAndTimerFacadeShould()
    {
        var _ = new FileAndTimerFacade(_fakeClient, _activityTimeoutIntervalInMilliseconds);
    }

    [Fact]
    public void SendTimeoutDetected()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 1.1));
        _fakeClient.Received(1).TimeoutDetected();
    }

    [Fact]
    public void SendActivityDetected()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        UpdateDirectory();
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        _fakeClient.Received(1).ActivityDetected();
        _fakeClient.Received(0).TimeoutDetected();
    }

    [Fact]
    public void NotReceiveTimeoutDetectedWhenUpdatingFile()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        UpdateDirectory();
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        UpdateDirectory();
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        _fakeClient.Received(0).TimeoutDetected();
    }

    [Fact]
    public void ReceiveMultipleTimeoutDetected()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 2.2));
        _fakeClient.Received(2).TimeoutDetected();
    }
}