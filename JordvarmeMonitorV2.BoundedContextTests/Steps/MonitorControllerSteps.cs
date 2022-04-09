﻿using JordvarmeMonitorV2.BoundedContextTests.Drivers;
using JordvarmeMonitorV2.BoundedContextTests.Support;
using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using TechTalk.SpecFlow;
using Xunit;

namespace JordvarmeMonitorV2.BoundedContextTests.Steps;

[Binding]
public class MonitorControllerSteps
{
    private readonly MonitorControllerContext _monitorControllerContext;
    private readonly MonitorControllerDriver _monitorControllerDriver;
    private readonly IActivityNotifications _fakeActivityNotifications;

    public MonitorControllerSteps(
        MonitorControllerContext monitorControllerContext
    )
    {
        _monitorControllerContext = monitorControllerContext;

        _fakeActivityNotifications = Substitute.For<IActivityNotifications>();

        _monitorControllerDriver = new MonitorControllerDriver( _fakeActivityNotifications);
    }

    [Given(@"the system is monitoring in (.*) Mode"), Scope(Feature = "Monitor the status of the Controller")]
    public void GivenTheSystemIsMonitoringInMode(Mode mode)
    {
        _monitorControllerContext.Mode = mode;

        _monitorControllerDriver.StubEventReceived(
            _monitorControllerContext.IsRunning 
                ? Event.Update 
                : Event.Timeout
        );
        _fakeActivityNotifications.ClearReceivedCalls();
    }

    [When(@"a (.*) event is received"), Scope(Feature = "Monitor the status of the Controller")]
    [When(@"an (.*) event is received"), Scope(Feature = "Monitor the status of the Controller")]
    public void WhenAEventIsReceived(Event @event)
    {
        _monitorControllerDriver.StubEventReceived(@event);
    }

    [Then(@"a single notification (.*) is sent"), Scope(Feature = "Monitor the status of the Controller")]
    public void ThenANotificationIsSent(Notification notification)
    {
        if (Notification.Stopped == notification)
        {
            _fakeActivityNotifications.Received(1).NotifyStopped();
        }
        else
        {
            _fakeActivityNotifications.Received(1).NotifyRunning();
        }
    }

    [Then(@"the Mode is changed to (.*)"), Scope(Feature = "Monitor the status of the Controller")]
    public void ThenTheModeIsChangedTo(Mode mode)
    {
        var result = _monitorControllerDriver.GetMode();
        Assert.Equal(mode,result);
    }

}