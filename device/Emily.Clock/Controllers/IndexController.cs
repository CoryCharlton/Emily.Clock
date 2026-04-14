using System;
using CCSWE.nanoFramework.Net;
using CCSWE.nanoFramework.WebServer;
using CCSWE.nanoFramework.WebServer.Authorization;
using Emily.Clock.Networking;

namespace Emily.Clock.Controllers;

[Route("/")]
[AllowAnonymous]
public class IndexController : ControllerBase
{
    private readonly IWirelessNetworkManager _wirelessNetworkManager;

    public IndexController(IWirelessNetworkManager wirelessNetworkManager)
    {
        _wirelessNetworkManager = wirelessNetworkManager;
    }
    
    [HttpGet("configure_alarm.html")]
    public void ConfigureAlarm() => Ok(Resources.GetBytes(Resources.BinaryResources.html_configure_alarm), MimeType.Text.Html);

    [HttpGet("configure_date_time.html")]
    public void ConfigureDateTime() => Ok(Resources.GetBytes(Resources.BinaryResources.html_configure_date_time), MimeType.Text.Html);

    [HttpGet("configure_night_light.html")]
    public void ConfigureNightLight() => Ok(Resources.GetBytes(Resources.BinaryResources.html_configure_night_light), MimeType.Text.Html);

    [HttpGet("configure_wireless_access_point.html")]
    public void ConfigureWirelessAccessPoint() => Ok(Resources.GetBytes(Resources.BinaryResources.html_configure_wireless_access_point), MimeType.Text.Html);

    [HttpGet("configure_wireless_client.html")]
    public void ConfigureWirelessClient() => Ok(Resources.GetBytes(Resources.BinaryResources.html_configure_wireless_client), MimeType.Text.Html);

    [HttpGet("favicon.ico")]
    public void Favicon() => Ok(Resources.GetBytes(Resources.BinaryResources.favicon2), "image/x-icon");
    
    [HttpGet]
    [HttpGet("index.html")]
    public void Index()
    {
        switch (_wirelessNetworkManager.GetMode())
        {
            case WirelessMode.Client:
                Ok(Resources.GetBytes(Resources.BinaryResources.html_index), MimeType.Text.Html);
                break;
            case WirelessMode.AccessPoint:
            default:
                Ok(Resources.GetBytes(Resources.BinaryResources.html_configure_wireless_client), MimeType.Text.Html);
                break;
        }
    }

    [HttpGet("js/app.js")]
    public void Script() => Ok(Resources.GetBytes(Resources.BinaryResources.script_app), MimeType.Text.JavaScript);
    
    [HttpGet("css/style.css")]
    public void Style() => Ok(Resources.GetBytes(Resources.BinaryResources.css_style), MimeType.Text.Css);
}