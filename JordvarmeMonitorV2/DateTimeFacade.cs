namespace JordvarmeMonitorV2;

/// <summary>
/// Enables to control time by providing fixed value returned by property Now.
/// Defaults to DateTime.Now
/// </summary>
[Obsolete]
public class DateTimeFacade
{
    private TimeSpan? _currentTime;

    public DateTimeFacade()
    {
        _currentTime = null;
    }

    public void SetTime(TimeSpan? currentTime)
    {
#if RELEASE
            throw new InvalidOperationException("Not allowed in Release Mode");
#endif
        _currentTime = currentTime;
    }

    public DateTime Now => _currentTime is null ? DateTime.Now : DateTime.Today.Add(_currentTime.Value);


}