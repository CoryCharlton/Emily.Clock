using System;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.Extensions;

[TestClass]
public class DateTimeExtensionsTests
{
    [TestMethod]
    public void Time_should_return_DateTime_with_zero_seconds()
    {
        var dateTime = new DateTime(2023, 10, 5, 14, 30, 45);

        var result = dateTime.Time();

        Assert.AreEqual(new DateTime(2023, 10, 5, 14, 30, 0), result);
    }
}