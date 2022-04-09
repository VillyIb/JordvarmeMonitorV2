﻿using JordvarmeMonitorV2.Contracts;
using NSubstitute;
using Xunit;
// ReSharper disable StringLiteralTypo

namespace JordvarmeMonitorV2.IntegrationTest;

[TestCaseOrderer("Alfa", "Bravo")]
public class FileAndTimerFacadeShould
{
    private readonly IFileSystemWatcherClient _fakeClient = Substitute.For<IFileSystemWatcherClient>();
    
    private static void UpdateDirectory()
    {
        var fileInfo = new FileInfo(Path.Combine(FileAndTimerFacade.WatchPath, "testfile.log"));
        using var sw = fileInfo.CreateText();
        sw.WriteLine("");
        sw.Flush();
        sw.Close();
    }

    public FileAndTimerFacadeShould()
    {
        var _ = new FileAndTimerFacade(_fakeClient);
    }

    [Fact]
    public void SendTimeoutDetected()
    {
        Thread.Sleep((int)(FileAndTimerFacade.IntervalInMilliseconds * 1.1));
        _fakeClient.Received(1).TimeoutDetected();
    }

    [Fact]
    public void SendActivityDetected()
    {
        Thread.Sleep(1000 * 1);
        UpdateDirectory();
        Thread.Sleep(1000 * 1);
        _fakeClient.Received().ActivityDetected();
    }

    [Fact]
    public void NotReceiveTimeoutDetectedWhenUpdatingFile()
    {
        Thread.Sleep((int)FileAndTimerFacade.IntervalInMilliseconds / 2);
        UpdateDirectory();
        Thread.Sleep((int)FileAndTimerFacade.IntervalInMilliseconds / 2);
        UpdateDirectory();
        Thread.Sleep((int)FileAndTimerFacade.IntervalInMilliseconds / 2);
        _fakeClient.Received(0).TimeoutDetected();
    }

    [Fact]
    public void ReceiveMultipleTimeoutDetected()
    {
        Thread.Sleep((int)FileAndTimerFacade.IntervalInMilliseconds * 2 + 1000 * 10);
        _fakeClient.Received(2).TimeoutDetected();
    }
}