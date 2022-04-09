// See https://aka.ms/new-console-template for more information

using JordvarmeMonitorV2;
using JordvarmeMonitorV2.Constants;
using JordvarmeMonitorV2.Transmit;

Console.WriteLine("Monitor start");

var emailSender = new EmailSender();
var notifications = new Notifications(emailSender);
var modeTarget = new HeartBeatMonitor(notifications, Settings.DurationBetweenHeartBeatStopped);
// ReSharper disable once ObjectCreationAsStatement
new HeartBeatGenerator(modeTarget, Settings.HeartBeatIntervalInMilliseconds);
var fileSystemWatchClient = new JordvarmeMonitorV2.ActivityMonitor(notifications, modeTarget);
// ReSharper disable once ObjectCreationAsStatement
new FileAndTimerFacade(fileSystemWatchClient, Settings.ActivityTimeoutIntervalInMilliseconds);

Console.ReadLine();
Console.WriteLine("Monitor stop");
