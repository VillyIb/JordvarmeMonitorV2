Feature: NotifyAlive

In order to know the Monitor is operational is a "Heart Beat" notification is sent at regular intervals.

Background: Time is 00:00:00
	Given the time is 00:00:00


Scenario: No HeartBeatOk notification before 06:00:00
	Given the system is monitoring in Running Mode
	And the time is 04:59:59
	When a HeartBeat-event is received
	And the time is 05:59:59
	When a HeartBeat-event is received
	Then 0 'HeartBeatOk' is sent

Scenario: Only one HeartBeatOk notification after 06:00:00
	Given the system is monitoring in Running Mode
	And the time is 06:00:01
	When a HeartBeat-event is received
	And the time is 23:59:00
	And a HeartBeat-event is received
	Then 1 'HeartBeatOk' is sent


Scenario: Only one HeartBeatStopped notification within one hour
	Given the system is monitoring in Stopped Mode
	And the time is 07:00:00
	When a HeartBeat-event is received
	And the time is 08:00:00
	And a HeartBeat-event is received
	Then 1 'HeartBeatStopped' is sent

Scenario: Multiple HeartBeatStopped notifications over multiple hours
	Given the system is monitoring in Stopped Mode
	And the time is 07:00:00
	When a HeartBeat-event is received
	And the time is 08:00:01
	And a HeartBeat-event is received
	Then 2 'HeartBeatStopped' are sent


Scenario: No HeartBeatStopped notification within one hour after a mode change to Stopped
	Given the system is monitoring in Running Mode
	And the time is 07:00:00
	And a Timeout event is received
	And the time is 08:00:00
	When a HeartBeat-event is received
	Then 0 'HeartBeatStopped' is sent

Scenario: One HeartBeatStopped notification after one hour after a mode change to Stopped
	Given the system is monitoring in Running Mode
	And the time is 07:00:00
	And a Timeout event is received
	And the time is 08:00:01
	When a HeartBeat-event is received
	Then 1 'HeartBeatStopped' is sent
