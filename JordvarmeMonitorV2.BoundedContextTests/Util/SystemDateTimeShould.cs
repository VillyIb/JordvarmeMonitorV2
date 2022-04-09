using System;
using JordvarmeMonitorV2.Util;
using Xunit;

namespace JordvarmeMonitorV2.BoundedContextTests.Util;

public class SystemDateTimeShould
{
    [Fact]
    public void ReturnRightKindForToday()
    {
        SystemDateTime.SetTime(new TimeSpan(06,00,00));

        var actual = SystemDateTime.Today;
        Assert.Equal(DateTimeKind.Local, actual.Kind);
    }

    [Theory]
    [InlineData(03, 26, 23)] // last day with standard time
    [InlineData(03, 27, 22)] // first day with summer time
    [InlineData(10, 29, 22)] // last day with summer time
    [InlineData(10, 30, 23)] // first day with standard time
    public void ReturnRightValueForToday(int month, int day, int hour)
    {
        SystemDateTime.SetTime(new DateTime(2022, month, day, hour,00,00, DateTimeKind.Utc), 0D);

        var expected = SystemDateTime.Today;
        var actual = SystemDateTime.Now;

        Assert.Equal(expected.Ticks,actual.Ticks);
    }
}