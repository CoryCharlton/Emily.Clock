using System;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;
using Emily.Clock.Configuration;
using nanoFramework.Json;

namespace Emily.Clock.Controllers
{
    public class ConfigurationController: ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConfigurationTypeFactory _configurationTypeFactory;

        public ConfigurationController(IConfigurationService configurationService, IConfigurationTypeFactory configurationTypeFactory)
        {
            _configurationService = configurationService;
            _configurationTypeFactory = configurationTypeFactory;
        }

        [Route("configuration/sections")]
        [Method("GET")]
        public void GetSections(WebServerEventArgs e) => Ok(e.Context.Response, _configurationService.GetSections());

        [Route("configuration/section/{name}")]
        [Method("GET")]
        public void GetSection(string name, WebServerEventArgs e)
        {
            var section = _configurationService.LoadConfigSection(name);
            if (string.IsNullOrEmpty(section))
            {
                NotFound(e.Context.Response);
                return;
            }

            Ok(e.Context.Response, section);
        }

        [Route("configuration/section/{name}")]
        [Method("POST")]
        public void PostSection(string name, WebServerEventArgs e)
        {
            try
            {
                var sectionType = _configurationTypeFactory.GetType(name);
                var configuration = JsonConvert.DeserializeObject(e.Context.Request.InputStream, sectionType);

                _configurationService.UpdateConfigSection(name, configuration);
            }
            catch (DeserializationException)
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

        /*
        [Route("configuration/section/{name}")]
        [Method("OPTIONS")]
        public void OptionsSection(string name, WebServerEventArgs e)
        {
            e.Context.Response.AddCors();
            MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.NoContent);
        }
        */


        /*
        [Route("configuration/exit")]
        [Method("GET")]
        public void Exit(WebServerEventArgs e)
        {
            e.Context.Response.AddCors();
            MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);

            new Thread(() =>
            {
                Thread.Sleep(5000);
                _configManager.StopConfigMode();
            }).Start();
        }
        */
    }
}
