﻿using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2;

using System.Timers;
using Util;

public class FileAndTimerFacade
{
    private readonly IFileSystemWatcherClient _client;

    private readonly Timer _timer;

    private void ResetTimer()
    {
        _timer.Stop();
        _timer.Start();
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine(Text.LabelReceivedChangedEvent, SystemDateTime.Now);
        ResetTimer();
        _client.ActivityDetected();
    }
    private void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine(Text.LabelReceivedTimeoutEvent, e.SignalTime, _timer.Interval);
        _client.TimeoutDetected();
    }

    public FileAndTimerFacade(IFileSystemWatcherClient client)
    {
        _client = client;

        var watcher = new FileSystemWatcher(Settings.WatchPath);
        watcher.Changed += OnChanged;
        watcher.Filter = "*.log";
        watcher.IncludeSubdirectories = false;

        _timer = new Timer(Settings.IntervalInMilliseconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;

        watcher.EnableRaisingEvents = true;
        _timer.Enabled = true;
    }
}