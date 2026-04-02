using CCSWE.nanoFramework.Net;
using CCSWE.nanoFramework.WebServer;
using CCSWE.nanoFramework.WebServer.Authorization;

namespace Emily.Clock.Controllers
{
    [Route("/")]
    [AllowAnonymous]
    public class IndexController : ControllerBase
    {
        private const string Html = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Emily.Clock</title>
    <style>
        body { font-family: sans-serif; max-width: 480px; margin: 2rem auto; padding: 0 1rem; }
        h1 { font-size: 1.4rem; margin-bottom: 1.5rem; }
        h2 { font-size: 1.1rem; margin-bottom: 1rem; }
        label { display: block; margin-bottom: 0.25rem; font-size: 0.9rem; }
        input { width: 100%; box-sizing: border-box; padding: 0.4rem; margin-bottom: 1rem; font-size: 1rem; }
        button { padding: 0.5rem 1.25rem; font-size: 1rem; cursor: pointer; }
        #status { margin-top: 0.75rem; font-size: 0.9rem; }
        #status.success { color: green; }
        #status.error { color: red; }
    </style>
</head>
<body>
    <h1>Emily.Clock</h1>
    <section>
        <h2>Wireless Client</h2>
        <form id=""wirelessClientForm"">
            <label for=""ssid"">SSID</label>
            <input type=""text"" id=""ssid"" name=""ssid"" required>

            <label for=""password"">Password</label>
            <input type=""password"" id=""password"" name=""password"">

            <label for=""connectionTimeout"">Connection Timeout (seconds)</label>
            <input type=""number"" id=""connectionTimeout"" name=""connectionTimeout"" required min=""1"">

            <button type=""submit"">Save</button>
            <div id=""status""></div>
        </form>
    </section>
    <script>
        (function () {
            var endpoint = '/api/configuration/wirelessclient';
            var form = document.getElementById('wirelessClientForm');
            var status = document.getElementById('status');

            fetch(endpoint)
                .then(function (r) { return r.json(); })
                .then(function (data) {
                    document.getElementById('ssid').value = data.Ssid || '';
                    document.getElementById('password').value = data.Password || '';
                    document.getElementById('connectionTimeout').value = data.ConnectionTimeout || '';
                })
                .catch(function () {
                    status.className = 'error';
                    status.textContent = 'Failed to load configuration.';
                });

            form.addEventListener('submit', function (e) {
                e.preventDefault();
                status.className = '';
                status.textContent = '';

                var payload = JSON.stringify({
                    Ssid: document.getElementById('ssid').value,
                    Password: document.getElementById('password').value,
                    ConnectionTimeout: parseInt(document.getElementById('connectionTimeout').value, 10)
                });

                fetch(endpoint, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: payload
                })
                .then(function (r) {
                    if (r.ok) {
                        status.className = 'success';
                        status.textContent = 'Saved.';
                    } else {
                        status.className = 'error';
                        status.textContent = 'Save failed (' + r.status + ').';
                    }
                })
                .catch(function () {
                    status.className = 'error';
                    status.textContent = 'Save failed.';
                });
            });
        })();
    </script>
</body>
</html>";

        [HttpGet]
        [HttpGet("index.html")]
        public void Index() => Ok(Html, MimeType.Text.Html);
    }
}
