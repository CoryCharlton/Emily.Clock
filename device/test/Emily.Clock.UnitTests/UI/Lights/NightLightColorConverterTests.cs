using System;
using System.Drawing;
using Emily.Clock.UI.Lights;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Lights;

[TestClass]
public class NightLightColorConverterTests
{
    [TestMethod]
    public void ToColor_should_return_color_for_Blue()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Blue);
        Assert.AreEqual(Color.Blue, result);
    }

    [TestMethod]
    public void ToColor_should_return_color_for_Green()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Green);
        Assert.AreEqual(Color.FromArgb(255, 0, 255, 0), result);
    }

    [TestMethod]
    public void ToColor_should_return_color_for_Indigo()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Indigo);
        Assert.AreEqual(Color.FromArgb(255, 255, 0, 255), result);
    }

    [TestMethod]
    public void ToColor_should_return_color_for_Orange()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Orange);
        Assert.AreEqual(Color.FromArgb(255, 255, 150, 0), result);
    }

    [TestMethod]
    public void ToColor_should_return_color_for_Red()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Red);
        Assert.AreEqual(Color.DarkRed, result);
    }

    [TestMethod]
    public void ToColor_should_return_color_for_Violet()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Violet);
        Assert.AreEqual(Color.DeepPink, result);
    }

    [TestMethod]
    public void ToColor_should_return_color_for_Yellow()
    {
        var result = NightLightColorConverter.ToColor(NightLightColor.Yellow);
        Assert.AreEqual(Color.Yellow, result);
    }

    [TestMethod]
    public void ToColor_should_throw_for_invalid_value()
    {
        Assert.ThrowsException(typeof(ArgumentOutOfRangeException), () =>
        {
            NightLightColorConverter.ToColor((NightLightColor)99);
        });
    }

    [TestMethod]
    public void ToString_should_return_string_for_Blue()
    {
        Assert.AreEqual("COLOR_BLUE", NightLightColorConverter.ToString(NightLightColor.Blue));
    }

    [TestMethod]
    public void ToString_should_return_string_for_Green()
    {
        Assert.AreEqual("COLOR_GREEN", NightLightColorConverter.ToString(NightLightColor.Green));
    }

    [TestMethod]
    public void ToString_should_return_string_for_Indigo()
    {
        Assert.AreEqual("COLOR_INDIGO", NightLightColorConverter.ToString(NightLightColor.Indigo));
    }

    [TestMethod]
    public void ToString_should_return_string_for_Orange()
    {
        Assert.AreEqual("COLOR_ORANGE", NightLightColorConverter.ToString(NightLightColor.Orange));
    }

    [TestMethod]
    public void ToString_should_return_string_for_Red()
    {
        Assert.AreEqual("COLOR_RED", NightLightColorConverter.ToString(NightLightColor.Red));
    }

    [TestMethod]
    public void ToString_should_return_string_for_Violet()
    {
        Assert.AreEqual("COLOR_VIOLET", NightLightColorConverter.ToString(NightLightColor.Violet));
    }

    [TestMethod]
    public void ToString_should_return_string_for_Yellow()
    {
        Assert.AreEqual("COLOR_YELLOW", NightLightColorConverter.ToString(NightLightColor.Yellow));
    }

    [TestMethod]
    public void ToString_should_throw_for_invalid_value()
    {
        Assert.ThrowsException(typeof(ArgumentOutOfRangeException), () =>
        {
            NightLightColorConverter.ToString((NightLightColor)99);
        });
    }
}
