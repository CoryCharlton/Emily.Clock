using System;
using System.Collections;
using CCSWE.nanoFramework.Configuration;

namespace Emily.Clock.Testing.Mocks;

public class ConfigurationManagerMock: IConfigurationManager
{
    private readonly Hashtable _configurations = new();

    public event ConfigurationChangedEventHandler? ConfigurationChanged;

    public void Clear()
    {
        _configurations.Clear();
    }

    public void Clear(string section)
    {
        var normalizedSection = NormalizeSection(section);

        if (_configurations.Contains(normalizedSection))
        {
            _configurations.Remove(normalizedSection);
        }
    }

    public bool Contains(string section) => _configurations.Contains(NormalizeSection(section));

    public object Get(string section) => _configurations[NormalizeSection(section)];

    public string[] GetSections()
    {
        var sections = new string[_configurations.Count];

        _configurations.Keys.CopyTo(sections, 0);

        return sections;
    }

    public Type GetType(string section)
    {
        if (!Contains(section))
        {
            throw new ArgumentException(nameof(section));
        }

        var configuration = Get(section);

        return configuration.GetType();
    }

    private static string NormalizeSection(string name) => name.ToLower();

    public void Save(string section, object configuration)
    {
        _configurations[NormalizeSection(section)] = configuration;

        ConfigurationChanged?.Invoke(this, new ConfigurationChangedEventArgs(section, configuration));
    }

    public void SaveAsync(string section, object configuration) => Save(section, configuration);
}