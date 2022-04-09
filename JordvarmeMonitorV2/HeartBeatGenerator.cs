using System.Timers;
using JordvarmeMonitorV2.Contracts;
using Timer = System.Timers.Timer;

namespace JordvarmeMonitorV2;

public class HeartBeatGenerator
{

#if DEBUG
    public static readonly double IntervalInMilliseconds = 1000 * 30;
#else
    public static readonly double IntervalInMilliseconds = 1000 * 60 * 2; // two minute
#endif

    private readonly IHeartBeatController _heartBeatController;

    private static Timer _timer = new();

    private void SetupTimer()
    {
        _timer = new Timer(IntervalInMilliseconds);
        _timer.Interval = IntervalInMilliseconds;
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public HeartBeatGenerator(IHeartBeatController heartBeatController)
    {
        _heartBeatController = heartBeatController;
        SetupTimer();
    }

    private void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine("{0:HH:mm:ss.fff} - The Elapsed heartbeat-event was raised ({1})", e.SignalTime, _timer.Interval);
        _heartBeatController.HeartBeat();
    }
}