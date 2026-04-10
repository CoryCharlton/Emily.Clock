using System.Collections;
using CCSWE.nanoFramework.Configuration;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Lights.Effects;

namespace Emily.Clock.Configuration;

public class NightLightConfiguration
{
    public const string Section = "NightLight";

    public float Brightness { get; set; } = 0.5f;

    public NightLightColor Color { get; set; } = NightLightColor.Violet;

    public NightLightEffectType Effect { get; set; } = NightLightEffectType.Solid;

    public float PanelBrightness { get; set; } = 0.5f;
}

public class NightLightConfigurationValidator : IValidateConfiguration
{
    public ValidateConfigurationResult Validate(object? configuration)
    {
        if (configuration is not NightLightConfiguration nightLightConfiguration)
        {
            return ValidateConfigurationResult.Fail("Configuration object is not the correct type");
        }

        var failures = new ArrayList();

        if (nightLightConfiguration.Brightness < 0.0f || nightLightConfiguration.Brightness > 1.0f)
        {
            failures.Add("Brightness must be between 0 and 1");
        }

        if ((int)nightLightConfiguration.Color < 0 || (int)nightLightConfiguration.Color > 6)
        {
            failures.Add("Color is not valid");
        }

        if ((int)nightLightConfiguration.Effect < 0 || (int)nightLightConfiguration.Effect > 2)
        {
            failures.Add("Effect is not valid");
        }

        if (nightLightConfiguration.PanelBrightness < 0.0f || nightLightConfiguration.PanelBrightness > 1.0f)
        {
            failures.Add("Panel brightness must be between 0 and 1");
        }

        return failures.Count > 0 ? ValidateConfigurationResult.Fail((string[]) failures.ToArray(typeof(string))) : ValidateConfigurationResult.Success;
    }
}
