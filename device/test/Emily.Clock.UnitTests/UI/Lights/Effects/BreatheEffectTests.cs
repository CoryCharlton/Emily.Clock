using System.Drawing;
using Emily.Clock.Device.Led;
using Emily.Clock.Testing.Mocks;
using Emily.Clock.UI.Lights.Effects;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Lights.Effects;

[TestClass]
public class BreatheEffectTests
{
    private static LedConfiguration CreateConfig() => new()
    {
        NightlightStartIndex = 2,
        NightlightEndIndex = 10
    };

    [TestMethod]
    public void Delay_should_be_25()
    {
        var effect = new BreatheEffect(Color.Blue, 1.0f);
        Assert.AreEqual(25, effect.Delay);
    }

    [TestMethod]
    public void Start_should_set_initial_brightness()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new BreatheEffect(Color.Blue, 1.0f);

        effect.Start(ledManager, config);

        Assert.AreEqual(1, ledManager.SetLedsCallCount);
    }

    [TestMethod]
    public void Step_should_cycle_and_return_true()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new BreatheEffect(Color.Blue, 1.0f);

        effect.Start(ledManager, config);

        for (var i = 0; i < 40; i++)
        {
            var result = effect.Step(ledManager, config);
            Assert.IsTrue(result);
        }

        Assert.AreEqual(41, ledManager.SetLedsCallCount);
    }

    [TestMethod]
    public void Stop_should_set_leds_to_black()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new BreatheEffect(Color.Blue, 1.0f);

        effect.Start(ledManager, config);
        effect.Stop(ledManager, config);

        Assert.AreEqual(Color.Black, ledManager.LastSetLedsColor);
    }
}
