using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;
using System.Timers;
using JordvarmeMonitorV2.Util;

namespace JordvarmeMonitorV2;

public class FileAndTimerFacade
{
    private readonly IActivityMonitor _activityMonitor;

    private System.Timers.Timer? _timer;

    private FileSystemWatcher? _watcher;

    private readonly double _intervalInMilliseconds;

    private void ResetTimer()
    {
        _timer?.Stop();
        _timer?.Start();
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine(Text.LabelReceivedChangedEvent, SystemDateTime.Now);
        ResetTimer();
        _activityMonitor.ActivityDetected();
    }
    private void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine(Text.LabelReceivedTimeoutEvent, e.SignalTime, _timer?.Interval);
        _activityMonitor.TimeoutDetected();
    }

    private void SetupFileSystemWatcher()
    {
        _watcher = new FileSystemWatcher(Settings.WatchPath);
        _watcher.Changed += OnChanged;
        _watcher.Filter = "*.log";
        _watcher.IncludeSubdirectories = false;
        _watcher.EnableRaisingEvents = true;
        _watcher.Error += ErrorEventHandler;
        _activityMonitor.Startup();
    }

    public void ErrorEventHandler(object sender, ErrorEventArgs e)
    {
        Console.WriteLine("FileSystemWatcher thrown exception: {0}, {1}",e.GetException(), e.GetException().StackTrace);
        _watcher?.Dispose();
        _watcher = null;
        SetupFileSystemWatcher();
    }

    private void SetupTimer()
    {
        _timer = new System.Timers.Timer(_intervalInMilliseconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public FileAndTimerFacade(IActivityMonitor activityMonitor, double intervalInMilliseconds)
    {
        _activityMonitor = activityMonitor;
        _intervalInMilliseconds = intervalInMilliseconds;

        SetupFileSystemWatcher();
        SetupTimer();
    }
}