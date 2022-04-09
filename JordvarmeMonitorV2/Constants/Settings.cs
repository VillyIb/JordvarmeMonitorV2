namespace JordvarmeMonitorV2.Constants
{
    public static class Settings
    {
#if DEBUG
        public static readonly double IntervalInMilliseconds = 1000 * 20; // 1/4 minute
        public static readonly string WatchPath = @"C:\Development\JordvarmeMonitor\MonitorDirectory\";

        public static readonly double HeartBeatIntervalInMilliseconds = 1000 * 60 * 2; // two minute
        public static readonly TimeSpan DurationBetweenHeartBeatStopped = new TimeSpan(0, 5, 0);
#else
        public static readonly double IntervalInMilliseconds = 1000 * 60 * 2; // two minute
        public static readonly string WatchPath = @"C:\GitRepositories\JordvarmeController\JordvarmeController\BoosterLog\";

        public static readonly double HeartBeatIntervalInMilliseconds = 1000 * 60 * 2; // two minute
        public static readonly TimeSpan DurationBetweenHeartBeatStopped = new TimeSpan(1, 0, 0);
#endif
    }
}
