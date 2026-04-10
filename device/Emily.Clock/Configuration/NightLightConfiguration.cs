using System.Collections;
using CCSWE.nanoFramework.Configuration;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Lights.Effects;

namespace Emily.Clock.Configuration;

public class NightLightConfiguration
{
    public const string Section = "NightLight";

    public static readonly NightLightConfiguration Defaults = new()
    {
        Brightness = 0.5f,
        Color = NightLightColor.Violet,
        Effect = NightLightEffectType.Solid,
        PanelBrightness = 0.5f
    };

    public float Brightness { get; set; } = -1.0f;

    public NightLightColor Color { get; set; } = (NightLightColor)(-1);

    public NightLightEffectType Effect { get; set; } = (NightLightEffectType)(-1);

    public float PanelBrightness { get; set; } = -1.0f;
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
