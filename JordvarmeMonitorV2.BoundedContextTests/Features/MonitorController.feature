Feature: Monitor the status of the Controller
	Mode: Running|Stopped
	Event: Timeout|Update
	Notification: Stopped|Started

@tag1
Scenario: Monitors Running Controller
	Given the system is monitoring in Running Mode
	When a Timeout event is received
	And a Timeout event is received
	Then a single notification Stopped is sent
	And the Mode is changed to Stopped

Scenario: Monitors Stopped Controller
	Given the system is monitoring in Stopped Mode
	When an Update event is received
	And an Update event is received
	Then a single notification Started is sent
	And the Mode is changed to Running
