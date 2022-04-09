namespace JordvarmeMonitorV2.Constants;

public static  class Text
{
    public const string LabelReceivedChangedEvent = "{0:HH:mm:ss.fff} - Received Changed event";

    public const string LabelReceivedTimeoutEvent = "{0:HH:mm:ss.fff} - Received timeout event ({1})";

    public const string LabelHeartBeatEvent = "{0:HH:mm:ss.fff} - Received heartbeat event ({1})";

    public const string RunningSubject = @"Jordvarme styring kører {0:HH\:mm}";
    public const string RunningBody = @"Jordvarme styring kører {0:HH\:mm}";

    public const string StoppedSubject = @"Jordvarme styring stoppet {0:HH\:mm}";
    public const string StoppedBodyUpToOneHour = @"Jordvarme styring har kl {0:HH\:mm} været stoppet i {1:%m} min.";
    public const string StoppedBodyOneToTwoHours = @"Jordvarme styring har kl {0:HH\:mm} været stoppet i {1:%h} time og {1:%m} min.";
    public const string StoppedBodyTwoHoursPlus = @"Jordvarme styring har kl {0:HH\:mm} været stoppet i {1:%h} timer og {1:%m} min.";

    public const string DailyMessage = @"Jordvarme styring kører fortsat {0:HH\:mm}";
}