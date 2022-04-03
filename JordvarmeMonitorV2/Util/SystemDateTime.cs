namespace JordvarmeMonitorV2.Util
{
    public static class SystemDateTime
    {
        #region Settings

        private static double Amplification { get; set; }


        private static DateTime RealStart { get; set; }


        private static DateTime Initial { get; set; }

        #endregion


        public static DateTime Now => UtcNow.ToLocalTime();


        /// <summary>
        /// Return the current date according to Local Time.
        /// </summary>
        public static DateTime Today => UtcNow.ToLocalTime().Date;


        /// <summary>
        /// Return Today+1.
        /// </summary>
        public static DateTime Tomorrow => UtcNow.ToLocalTime().Date.AddDays(1);


        /// <summary>
        /// Return Today-1
        /// </summary>
        public static DateTime Yesterday => UtcNow.ToLocalTime().Date.AddDays(-1);


        public static DateTime UtcNow
        {
            get
            {
                var realDuration = DateTime.UtcNow.Subtract(RealStart);

                var realDurationTicks = realDuration.Ticks;

                var simulatedDurationTicks = (long)(realDurationTicks * Amplification);

                var result = Initial.AddTicks(simulatedDurationTicks);

                return result;
            }
        }


        /// <summary>
        /// Sets DateTime (Default = Now) and optionally set the Amplification (Default = realtime).
        /// An initialUtcDateTime of null selects the current DateTime.
        /// Amplification:
        /// - 1.0D means real time speed (Default)
        /// - 2.0D means the time runs at double speed.
        /// - 0.5D means the time runs at half speed.
        /// - 0.0D means the time stands still.
        /// - -1.0D means time runs backwards at real time speed.
        /// </summary>
        /// <param name="initialUtcDateTime"></param>
        /// <param name="amplification"></param>
        public static void SetTime(DateTime? initialUtcDateTime, double amplification = 1.0D)
        {
            RealStart = DateTime.UtcNow;
            Initial = initialUtcDateTime ?? RealStart;
            Amplification = amplification;
        }

        /// <summary>
        /// Sets the DateTime to Today at the specified (local)time and optionally sets the Amplification (Default = stopped).
        /// </summary>
        /// <param name="nowLocalTime"></param>
        /// <param name="amplification"></param>
        public static void SetTime(TimeSpan nowLocalTime, double amplification = 0.0D)
        {
            RealStart = DateTime.UtcNow;
            Initial = DateTime.Today.Add(nowLocalTime).ToUniversalTime();
            Amplification = amplification;
        }


        /// <summary>
        /// Resets the SystemTimeStub to use the (MS) System.DateTime.
        /// </summary>
        public static void Reset()
        {
            SetTime(null);
        }


        static SystemDateTime()
        {
            Reset();
        }
    }

}
