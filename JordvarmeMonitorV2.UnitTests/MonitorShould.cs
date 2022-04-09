using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;

namespace JordvarmeMonitorV2.UnitTests
{
    public  class MonitorShould
    {
        private readonly  Monitor _sut;
        private readonly  INotifications _fakeNotifications = Substitute.For<INotifications>();

        public MonitorShould()
        {
            _sut = new Monitor(_fakeNotifications, null);
        }

        [Fact]
        public void UnconditionallySendRunningUponFirstActivityDetected()
        {
            _sut.ActivityDetected();
            _fakeNotifications.Received(1).NotifyRunning();
        }

        [Fact]
        public void OnlySendOneRunningUponMultipleActivityDetected()
        {
            _sut.ActivityDetected();
            _sut.ActivityDetected();
            _sut.ActivityDetected();
            _fakeNotifications.Received(1).NotifyRunning();
        }

        [Fact]
        public void UnconditionallySendStoppedUponFirstTimeoutDetected()
        {
            _sut.TimeoutDetected();
            _fakeNotifications.Received(1).NotifyStopped();
        }

        [Fact]
        public void OnlySendOneStoppedUponMultipleActivityDetected()
        {
            _sut.TimeoutDetected();
            _sut.TimeoutDetected();
            _sut.TimeoutDetected();
            _fakeNotifications.Received(1).NotifyStopped();
        }

    }
}
