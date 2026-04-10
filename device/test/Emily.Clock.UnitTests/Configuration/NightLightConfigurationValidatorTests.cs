using Emily.Clock.Configuration;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Lights.Effects;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.Configuration;

[TestClass]
public class NightLightConfigurationValidatorTests
{
    [TestMethod]
    public void Validate_should_return_failure_when_Brightness_is_negative()
    {
        var config = new NightLightConfiguration
        {
            Brightness = -0.1f,
            Color = NightLightColor.Violet,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = 0.5f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Brightness must be between 0 and 1", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_Brightness_exceeds_1()
    {
        var config = new NightLightConfiguration
        {
            Brightness = 1.1f,
            Color = NightLightColor.Violet,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = 0.5f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Brightness must be between 0 and 1", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_Color_is_invalid()
    {
        var config = new NightLightConfiguration
        {
            Brightness = 0.5f,
            Color = (NightLightColor)7,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = 0.5f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Color is not valid", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_configuration_is_not_NightLightConfiguration()
    {
        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(new object());

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Configuration object is not the correct type", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_Effect_is_invalid()
    {
        var config = new NightLightConfiguration
        {
            Brightness = 0.5f,
            Color = NightLightColor.Violet,
            Effect = (NightLightEffectType)3,
            PanelBrightness = 0.5f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Effect is not valid", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_PanelBrightness_is_negative()
    {
        var config = new NightLightConfiguration
        {
            Brightness = 0.5f,
            Color = NightLightColor.Violet,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = -0.1f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Panel brightness must be between 0 and 1", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_PanelBrightness_exceeds_1()
    {
        var config = new NightLightConfiguration
        {
            Brightness = 0.5f,
            Color = NightLightColor.Violet,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = 1.1f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Panel brightness must be between 0 and 1", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_multiple_failures_when_multiple_fields_are_invalid()
    {
        var config = new NightLightConfiguration
        {
            Brightness = -0.1f,
            Color = NightLightColor.Violet,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = 1.5f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(2, result.Failures.Length);
    }

    [TestMethod]
    public void Validate_should_return_success_when_configuration_is_valid()
    {
        var config = new NightLightConfiguration
        {
            Brightness = 0.5f,
            Color = NightLightColor.Violet,
            Effect = NightLightEffectType.Solid,
            PanelBrightness = 0.5f
        };

        var validator = new NightLightConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsFalse(result.Failed);
        Assert.IsTrue(result.Succeeded);
    }
}
