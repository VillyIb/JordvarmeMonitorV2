using System.Timers;
using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;
using Timer = System.Timers.Timer;

namespace JordvarmeMonitorV2;

public class HeartBeatGenerator
{
    private readonly IHeartBeatMonitor _heartBeatMonitor;

    private readonly Timer _timer = new ();
    private readonly double _heartBeatIntervalInMilliseconds ;

    private void SetupTimer()
    {
        _timer.Interval = _heartBeatIntervalInMilliseconds;
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public HeartBeatGenerator(IHeartBeatMonitor heartBeatMonitor, double heartBeatIntervalInMilliseconds)
    {
        _heartBeatMonitor = heartBeatMonitor;
        _heartBeatIntervalInMilliseconds = heartBeatIntervalInMilliseconds;
        SetupTimer();
    }

    private void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine(Text.LabelHeartBeatEvent, e.SignalTime, _timer.Interval);
        _heartBeatMonitor.HeartBeat();
    }
}