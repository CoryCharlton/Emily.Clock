using System.Collections;

namespace Emily.Clock.Configuration
{
    public class ConfigurationValidationResults
    {
        public ConfigurationValidationResults()
        {
            IsValid = true;
        }

        public ConfigurationValidationResults(string failure)
        {
            AddFailure(failure);
        }

        public ConfigurationValidationResults(bool isValid)
        {
            IsValid = isValid;

            if (!IsValid)
            {
                AddFailure("Configuration validation failed");
            }
        }

        public ArrayList Failures { get; } = new();

        public bool IsValid { get; private set; }

        public void AddFailure(string failure)
        {
            if (string.IsNullOrEmpty(failure))
            {
                return;
            }

            Failures.Add(failure);
            IsValid = false;
        }
    }
}
