using System;
using System.Globalization;
using JordvarmeMonitorV2.BoundedContextTests.Drivers;
using JordvarmeMonitorV2.BoundedContextTests.Support;
using JordvarmeMonitorV2.Constants;
using NSubstitute;
using TechTalk.SpecFlow;

namespace JordvarmeMonitorV2.BoundedContextTests.Steps
{
    [Binding]
    public class NotifyAliveSteps
    {
        private readonly MonitorControllerContext _monitorControllerContext;
        private readonly NotifyAliveDriver _monitorControllerDriver;
        private readonly IHeartBeatNotifications _fakeHeartbeatNotifications;
        private readonly DateTimeFacade _dateTimeFacade;

        public NotifyAliveSteps(MonitorControllerContext monitorControllerContext)
        {
            _monitorControllerContext = monitorControllerContext;

            _dateTimeFacade = new DateTimeFacade();

            var fakeNotifications = Substitute.For<INotifications>();

            _fakeHeartbeatNotifications = Substitute.For<IHeartBeatNotifications>();

            _monitorControllerDriver = new NotifyAliveDriver(fakeNotifications, _fakeHeartbeatNotifications, _dateTimeFacade);
        }

        [Given(@"the system is monitoring in (.*) Mode"), Scope(Feature = "NotifyAlive")]
        public void GivenTheSystemIsMonitoringInMode(Mode mode)
        {
            _monitorControllerContext.Mode = mode;

            _monitorControllerDriver.StubEventReceived(
                _monitorControllerContext.IsRunning
                    ? Event.Update
                    : Event.Timeout
            );
            _monitorControllerDriver.StubNotifyEvent();
            _fakeHeartbeatNotifications.ClearReceivedCalls();
        }

        [Given(@"the time is (.*)"), Scope(Feature = "NotifyAlive")]
        [When(@"the time is (.*)"), Scope(Feature = "NotifyAlive")]
        public void GivenTheTimeIs(string input)
        {
            var formatProvider = CultureInfo.InvariantCulture;
            var format = "g";
            var styles = TimeSpanStyles.None;
            _dateTimeFacade.SetTime(TimeSpan.ParseExact(input, format, formatProvider, styles));
        }

        [Then(@"(.*) '([^']*)' is sent"), Scope(Feature = "NotifyAlive")]
        [Then(@"(.*) '([^']*)' are sent"), Scope(Feature = "NotifyAlive")]
        //[Then(@"(.*) (.*) are sent"), Scope(Feature = "NotifyAlive")]
        public void ThenHeartBeatsStoppedAreSent(int p0, HeartBeatEvents heartBeatEvent)
        {
            switch (heartBeatEvent)
            {
                case HeartBeatEvents.HeartBeatOk:
                    {
                        _fakeHeartbeatNotifications.Received(p0).HeartBeatOk();
                        break;
                    }
                case HeartBeatEvents.HeartBeatStopped:

                    {
                        _fakeHeartbeatNotifications.Received(p0).HeartBeatStopped();
                        break;
                    }
            }
        }

        [Given(@"a (.*) event is received"), Scope(Feature = "NotifyAlive")]
        public void GivenATimeoutEventIsReceived(Event @event)
        {
            _monitorControllerDriver.StubEventReceived(@event);
        }


        [When(@"a HeartBeat-event is received"), Scope(Feature = "NotifyAlive")]
        public void WhenAHeartBeatEventIsReceived()
        {
            _monitorControllerDriver.StubNotifyEvent();
        }
    }
}
