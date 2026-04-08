using System.Drawing;
using Emily.Clock.Device.Led;
using Emily.Clock.Testing.Mocks;
using Emily.Clock.UI.Lights.Effects;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Lights.Effects;

[TestClass]
public class RainbowEffectTests
{
    private static LedConfiguration CreateConfig() => new()
    {
        NightlightStartIndex = 2,
        NightlightEndIndex = 10
    };

    [TestMethod]
    public void Delay_should_be_20()
    {
        var effect = new RainbowEffect();
        Assert.AreEqual(20, effect.Delay);
    }

    [TestMethod]
    public void Start_should_set_leds()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new RainbowEffect();

        effect.Start(ledManager, config);

        Assert.IsTrue(ledManager.SetLedCallCount > 0);
    }

    [TestMethod]
    public void Step_should_advance_and_return_true()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new RainbowEffect();

        effect.Start(ledManager, config);
        var countAfterStart = ledManager.SetLedCallCount;

        var result = effect.Step(ledManager, config);
        Assert.IsTrue(result);
        Assert.IsTrue(ledManager.SetLedCallCount > countAfterStart);
    }

    [TestMethod]
    public void Stop_should_set_leds_to_black()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new RainbowEffect();

        effect.Start(ledManager, config);
        effect.Stop(ledManager, config);

        Assert.AreEqual(Color.Black, ledManager.LastSetLedsColor);
    }
}
