using nanoFramework.TestFramework;
using System;

namespace Emily.Clock.UnitTests.Extensions;

[TestClass]
public class TimeSpanExtensionsTests
{
    [TestMethod]
    public void ToShortString_TimeSpan_Zero_returns_correct_format()
    {
        var timeSpan = TimeSpan.Zero;
        var result = timeSpan.ToShortString();
        Assert.AreEqual("00:00:00", result);
    }

    [TestMethod]
    public void ToShortString_with_days_returns_correct_format()
    {
        var timeSpan = new TimeSpan(99, 10, 20, 30, 400);
        var result = timeSpan.ToShortString();
        Assert.AreEqual("99.10:20:30", result);
    }

    [TestMethod]
    public void ToShortString_with_no_days_returns_correct_format()
    {
        var timeSpan = new TimeSpan(0, 5, 15, 25, 300);
        var result = timeSpan.ToShortString();
        Assert.AreEqual("05:15:25", result);
    }
}