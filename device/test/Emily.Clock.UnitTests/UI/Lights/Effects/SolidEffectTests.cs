using System.Drawing;
using System.Threading;
using Emily.Clock.Device.Led;
using Emily.Clock.Testing.Mocks;
using Emily.Clock.UI.Lights.Effects;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Lights.Effects;

[TestClass]
public class SolidEffectTests
{
    private static LedConfiguration CreateConfig() => new()
    {
        NightlightStartIndex = 2,
        NightlightEndIndex = 10
    };

    [TestMethod]
    public void Delay_should_be_Infinite()
    {
        var effect = new SolidEffect(Color.Blue, 0.5f);
        Assert.AreEqual(Timeout.Infinite, effect.Delay);
    }

    [TestMethod]
    public void Start_should_set_leds_to_color()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new SolidEffect(Color.Red, 1.0f);

        effect.Start(ledManager, config);

        Assert.AreEqual(1, ledManager.SetLedsCallCount);
    }

    [TestMethod]
    public void Step_should_return_false()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new SolidEffect(Color.Red, 1.0f);

        effect.Start(ledManager, config);
        var result = effect.Step(ledManager, config);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Stop_should_set_leds_to_black()
    {
        var ledManager = new LedManagerMock();
        var config = CreateConfig();
        var effect = new SolidEffect(Color.Red, 1.0f);

        effect.Start(ledManager, config);
        effect.Stop(ledManager, config);

        Assert.AreEqual(Color.Black, ledManager.LastSetLedsColor);
    }
}
