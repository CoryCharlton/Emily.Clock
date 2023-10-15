using System;
using System.Collections;

namespace Emily.Clock.Configuration
{
    public interface IConfigurationTypeFactory
    {
        void AddType(string section, Type type);
        Type GetType(string section);
    }

    public class ConfigurationTypeFactory : IConfigurationTypeFactory
    {
        private readonly Hashtable _typesBySectionName = new();

        public ConfigurationTypeFactory()
        {
            AddType(DateTimeConfiguration.SectionName, typeof(DateTimeConfiguration));    
            AddType(WirelessAccessPointConfiguration.SectionName, typeof(WirelessAccessPointConfiguration));    
            AddType(WirelessClientConfiguration.SectionName, typeof(WirelessClientConfiguration));
        }

        public void AddType(string section, Type type)
        {
            var lowerCaseSection = section.ToLower();

            if (_typesBySectionName.Contains(lowerCaseSection))
            {
                throw new ArgumentException();
            }

            _typesBySectionName[lowerCaseSection] = type;
        }

        public Type GetType(string section)
        {
            if (_typesBySectionName[section] is not Type sectionType)
            {
                throw new ArgumentException();
            }

            return sectionType;
        }
    }
}
