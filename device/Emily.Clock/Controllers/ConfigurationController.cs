using System;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.WebServer;
using CCSWE.nanoFramework.WebServer.Evaluate;
using nanoFramework.Json;

namespace Emily.Clock.Controllers
{
    public class ConfigurationController: ControllerBase
    {
        private readonly IConfigurationManager _configurationManager;

        public ConfigurationController(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        [Route("configuration/{name}")]
        [Method("GET")]
        public void GetConfiguration(string name, WebServerEventArgs e)
        {
            if (!_configurationManager.Contains(name))
            {
                NotFound(e.Context.Response);
                return;
            }

            try
            {
                Ok(e.Context.Response, _configurationManager.Get(name));
            }
            catch (Exception)
            {
                InternalServerError(e.Context.Response);
            }
        }

        [Route("configuration/")]
        [Method("GET")]
        public void GetSections(WebServerEventArgs e) => Ok(e.Context.Response, _configurationManager.GetSections());

        [Route("configuration/{section}")]
        [Method("POST")]
        public void SaveConfiguration(string section, WebServerEventArgs e)
        {
            if (!_configurationManager.Contains(section))
            {
                NotFound(e.Context.Response);
                return;
            }

            try
            {
                var type = _configurationManager.GetType(section);
                var configuration = JsonConvert.DeserializeObject(e.Context.Request.InputStream, type);

                _configurationManager.Save(section, configuration);
            }
            catch (DeserializationException)
            {
                BadRequest(e.Context.Response);
                return;
            }
            catch (ValidateConfigurationException)
            {
                BadRequest(e.Context.Response);
                return;
            }
            catch (Exception)
            {
                InternalServerError(e.Context.Response);
                return;
            }

            Ok(e.Context.Response);
        }
    }
}
