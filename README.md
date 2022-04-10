JordvarmeMonitorV2

Sends an email if a monitored process stops working.

Monitors the process by monitoring the logifle of the process. If no changes to the logfile for more than 2 minutes sends a "stopped" notification.

Sends a "started" notification if started again.

Sends a "stopped for duration" notification every hour it's stopped

Sends a "i'm alive" notification once every day.

Handles a flaw in FileSystemWatcher by restarting upon error.
