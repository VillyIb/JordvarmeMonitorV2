Feature: NotifyAlive

In order to know the Monitor is operational is a "Heart Beat" notification sent at regular intervals.

Background: Time is 00:00:00
	Given the time is 00:00:00


Scenario: No HeartBeatOk notification before 06:00:00
	Given the system is monitoring in Running Mode
	And the time is 04:59:59
	When a HeartBeat-event is received
	And the time is 05:59:59
	When a HeartBeat-event is received
	Then 0 'HeartBeatOk' is sent

Scenario: One HeartBeatOk notification after 06:00:00
	Given the system is monitoring in Running Mode
	And the time is 06:00:00
	When a HeartBeat-event is received
	Then 1 'HeartBeatOk' is sent

Scenario: Only one HeartBeatOk from multiple notifications
	Given the system is monitoring in Running Mode
	And the time is 06:00:00
	When a HeartBeat-event is received
	And the time is 23:59:59
	And a HeartBeat-event is received
	Then 1 'HeartBeatOk' is sent

Scenario: One HeartBeatOk notification after mode has changed to Running
	Given the system is monitoring in Stopped Mode
	And the time is 04:00:00
	And an Update event is received
	And the time is 06:00:00
	When a HeartBeat-event is received
	Then 1 'HeartBeatOk' are sent

Scenario: Only one HeartBeatStopped notification within one 'DurationBetweenHeartBeatStopped'
	Given the system is monitoring in Stopped Mode
	And the time is 07:00:00
	When a HeartBeat-event is received
	And the time is 07:04:59
	And a HeartBeat-event is received
	Then 1 'HeartBeatStopped' is sent

Scenario: Multiple HeartBeatStopped notifications over multiple 'DurationBetweenHeartBeatStopped'
	Given the system is monitoring in Stopped Mode
	And the time is 07:00:00
	When a HeartBeat-event is received
	And the time is 07:05:00
	And a HeartBeat-event is received
	Then 2 'HeartBeatStopped' are sent

Scenario: No HeartBeatStopped notification within one 'DurationBetweenHeartBeatStopped' after a mode change to Stopped
	Given the system is monitoring in Running Mode
	And the time is 07:00:00
	And a Timeout event is received
	And the time is 07:04:59
	When a HeartBeat-event is received
	Then 0 'HeartBeatStopped' is sent

Scenario: One HeartBeatStopped notification after one 'DurationBetweenHeartBeatStopped' after a mode change to Stopped
	Given the system is monitoring in Running Mode
	And the time is 07:00:00
	And a Timeout event is received
	And the time is 07:05:00
	When a HeartBeat-event is received
	Then 1 'HeartBeatStopped' is sent
