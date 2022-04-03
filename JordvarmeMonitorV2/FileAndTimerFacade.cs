
using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Util;

namespace JordvarmeMonitorV2;

using System.Timers;

public interface IFileSystemWatcherClient
{
    void ActivityDetected();

    void TimeoutDetected();

    bool IsRunning { get; }
}

public  class FileAndTimerFacade
{
#if DEBUG
    private static readonly double IntervalInMiliSeconds = 1000 * 30; // half minute
    private static readonly string WatchPath = @"C:\Development\JordvarmeMonitor\MonitorDirectory\";
#else
        private static readonly double IntervalInMiliSeconds = 1000 * 60 * 2; // two minute
        private static readonly string WatchPath = @"C:\GitRepositories\JordvarmeController\JordvarmeController\BoosterLog\";
#endif
    private readonly IFileSystemWatcherClient _client;

    private readonly System.Timers.Timer _timer;

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
    private  void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine("{0:HH:mm:ss.fff} - The Elapsed event was raised ({1})", e.SignalTime, _timer.Interval);
        _client.TimeoutDetected();
    }

    public FileAndTimerFacade(IFileSystemWatcherClient client)
    {
        _client = client;

        var watcher = new FileSystemWatcher(WatchPath);
        watcher.Changed += OnChanged;
        watcher.Filter = "*.log";
        watcher.IncludeSubdirectories = false;
            
        _timer = new System.Timers.Timer(IntervalInMiliSeconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
            
        watcher.EnableRaisingEvents = true;
        _timer.Enabled = true;
    }
}