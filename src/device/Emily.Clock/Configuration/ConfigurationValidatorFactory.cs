using System;
using System.Collections;

namespace Emily.Clock.Configuration
{
    internal interface IConfigurationValidator
    {
        ConfigurationValidationResults ValidateConfiguration(object configuration);
    }

    public interface IConfigurationValidatorFactory
    {
        //void AddValidator(string section, IConfigurationValidator validator);
        //IConfigurationValidator GetValidator(string section);
        ConfigurationValidationResults ValidateConfiguration(string sectionName, object configuration);
    }

    internal class ConfigurationValidatorFactory : IConfigurationValidatorFactory
    {
        private readonly Hashtable _validatorsBySectionName = new();

        public ConfigurationValidatorFactory()
        {
            AddValidator(DateTimeConfiguration.SectionName, new DateTimeConfigurationValidator());
            /*
            AddValidator(NightLightConfiguration.SectionName, typeof(NightLightConfiguration));
            AddValidator(WirelessAccessPointConfiguration.SectionName, typeof(WirelessAccessPointConfiguration));
            AddValidator(WirelessClientConfiguration.SectionName, typeof(WirelessClientConfiguration));
            */
        }

        internal void AddValidator(string section, IConfigurationValidator validator)
        {
            var lowerCaseSection = section.ToLower();

            if (_validatorsBySectionName.Contains(lowerCaseSection))
            {
                throw new ArgumentException();
            }

            _validatorsBySectionName[lowerCaseSection] = validator;
        }

        internal IConfigurationValidator GetValidator(string section)
        {
            return _validatorsBySectionName[section] as IConfigurationValidator;
        }

        public ConfigurationValidationResults ValidateConfiguration(string sectionName, object configuration)
        {
            return GetValidator(sectionName) is not { } validator ? new ConfigurationValidationResults() : validator.ValidateConfiguration(configuration);
        }
    }

}
