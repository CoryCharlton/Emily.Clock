using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.WebServer;
using nanoFramework.Json;

namespace Emily.Clock.Controllers
{
    [Route("/api/configuration")]
    public class ConfigurationController: ControllerBase
    {
        private readonly IConfigurationManager _configurationManager;

        public ConfigurationController(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        [HttpGet("{section}")]
        public void GetConfiguration(string section)
        {
            if (!_configurationManager.Contains(section))
            {
                NotFound();
                return;
            }

            Ok(_configurationManager.Get(section));
        }

        [HttpGet]
        public void GetSections() => Ok(_configurationManager.GetSections());

        [HttpPost("{section}")]
        public void SaveConfiguration(string section)
        {
            if (!_configurationManager.Contains(section))
            {
                NotFound();
                return;
            }

            try
            {
                // TODO: Check content type to switch between JSON and FORM?
                var type = _configurationManager.GetType(section);
                var configuration = JsonConvert.DeserializeObject(Request.Body, type);

                _configurationManager.Save(section, configuration);
            }
            catch (DeserializationException)
            {
                BadRequest();
                return;
            }
            catch (ValidateConfigurationException)
            {
                BadRequest();
                return;
            }

            Ok();
        }
    }
}
