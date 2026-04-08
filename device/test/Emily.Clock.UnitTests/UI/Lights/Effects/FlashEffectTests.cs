using System.Drawing;
using Emily.Clock.Device.Led;
using Emily.Clock.Testing.Mocks;
using Emily.Clock.UI.Lights.Effects;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Lights.Effects;

[TestClass]
public class FlashEffectTests
{
    private static LedConfiguration CreateConfig() => new()
    {
        NightlightStartIndex = 2,
        NightlightEndIndex = 10
    };

    [TestMethod]
    public void Delay_should_match_configured_value()
    {
        var effect = new FlashEffect(Color.Red, Color.Black, 500);
        Assert.AreEqual(500, effect.Delay);
    }

    [TestMethod]
    public void Start_should_set_leds_to_on_color()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new FlashEffect(Color.Red, Color.Black, 500);

        effect.Start(ledManager, config);

        Assert.AreEqual(Color.Red, ledManager.LastSetLedsColor);
    }

    [TestMethod]
    public void Step_should_alternate_colors_and_return_true()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new FlashEffect(Color.Red, Color.Black, 500);

        effect.Start(ledManager, config);

        var result1 = effect.Step(ledManager, config);
        Assert.IsTrue(result1);
        Assert.AreEqual(Color.Black, ledManager.LastSetLedsColor);

        var result2 = effect.Step(ledManager, config);
        Assert.IsTrue(result2);
        Assert.AreEqual(Color.Red, ledManager.LastSetLedsColor);
    }

    [TestMethod]
    public void Stop_should_set_leds_to_black()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new FlashEffect(Color.Red, Color.Blue, 500);

        effect.Start(ledManager, config);
        effect.Stop(ledManager, config);

        Assert.AreEqual(Color.Black, ledManager.LastSetLedsColor);
    }
}
