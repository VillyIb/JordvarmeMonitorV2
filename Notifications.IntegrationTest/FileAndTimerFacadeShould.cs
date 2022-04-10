using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;
// ReSharper disable StringLiteralTypo

namespace JordvarmeMonitorV2.IntegrationTest;

[TestCaseOrderer("Alfa", "Bravo")]
public class FileAndTimerFacadeShould
{
    private readonly IActivityMonitor _fakeActivityMonitor = Substitute.For<IActivityMonitor>();

    private double _activityTimeoutIntervalInMilliseconds = 1000 * 2;

    private readonly FileAndTimerFacade _sut;


    private static void UpdateDirectory()
    {
        var fileInfo = new FileInfo(Path.Combine(Settings.WatchPath, "testfile.log"));
        using var sw = fileInfo.CreateText();
        sw.Flush();
        sw.Close();
    }

    public FileAndTimerFacadeShould()
    {
        _sut = new FileAndTimerFacade(_fakeActivityMonitor, _activityTimeoutIntervalInMilliseconds);
    }

    [Fact]
    public void SendTimeoutDetected()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 1.1));
        _fakeActivityMonitor.Received(1).TimeoutDetected();
    }

    [Fact]
    public void SendActivityDetected()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        UpdateDirectory();
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        _fakeActivityMonitor.Received(1).ActivityDetected();
        _fakeActivityMonitor.Received(0).TimeoutDetected();
    }

    [Fact]
    public void NotReceiveTimeoutDetectedWhenUpdatingFile()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        UpdateDirectory();
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        UpdateDirectory();
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 0.9));
        _fakeActivityMonitor.Received(0).TimeoutDetected();
    }

    [Fact]
    public void ReceiveMultipleTimeoutDetected()
    {
        Thread.Sleep((int)(_activityTimeoutIntervalInMilliseconds * 2.2));
        _fakeActivityMonitor.Received(2).TimeoutDetected();
    }

    [Fact]
    public void NotifyStartupUponStartup()
    {
        _fakeActivityMonitor.Received(1).Startup();
    }

    [Fact]
    public void IssueRestartUponErrorEvent()
    {
        _fakeActivityMonitor.ClearReceivedCalls();
        _sut.ErrorEventHandler(this,  new ErrorEventArgs(new ApplicationException("UnitTest")) );
        _fakeActivityMonitor.Received(1).Startup();
    }
}