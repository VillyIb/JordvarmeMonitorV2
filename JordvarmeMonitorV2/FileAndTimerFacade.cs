
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2;

using System.Timers;
using Util;

public class FileAndTimerFacade
{
#if DEBUG
    public static readonly double IntervalInMilliseconds = 1000 * 20; // 1/4 minute
    public static readonly string WatchPath = @"C:\Development\JordvarmeMonitor\MonitorDirectory\";
#else
        public static readonly double IntervalInMilliseconds = 1000 * 60 * 2; // two minute
        public static readonly string WatchPath = @"C:\GitRepositories\JordvarmeController\JordvarmeController\BoosterLog\";
#endif
    private readonly IFileSystemWatcherClient _client;

    private readonly Timer _timer;

    private void ResetTimer()
    {
        _timer.Stop();
        _timer.Start();
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine("{0:HH:mm:ss.fff} - Received Changed event", SystemDateTime.Now);
        ResetTimer();
        _client.ActivityDetected();
    }
    private void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine("{0:HH:mm:ss.fff} - The Elapsed timeout-event was raised ({1})", e.SignalTime, _timer.Interval);
        _client.TimeoutDetected();
    }

    public FileAndTimerFacade(IFileSystemWatcherClient client)
    {
        _client = client;

        var watcher = new FileSystemWatcher(WatchPath);
        watcher.Changed += OnChanged;
        watcher.Filter = "*.log";
        watcher.IncludeSubdirectories = false;

        _timer = new Timer(IntervalInMilliseconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;

        watcher.EnableRaisingEvents = true;
        _timer.Enabled = true;
    }
}