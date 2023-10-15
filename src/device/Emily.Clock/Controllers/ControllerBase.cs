using nanoFramework.Json;
using System.Net;
using System.Text;
using MakoIoT.Device.Services.Server.Extensions;

namespace Emily.Clock.Controllers
{
    public abstract class ControllerBase
    {
        private static void AddJsonOutput(HttpListenerResponse response, object body)
        {
            if (body is null)
            {
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(body as string ?? JsonConvert.SerializeObject(body));

            response.ContentLength64 = bytes.Length;
            response.ContentType = "application/json";
            response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        protected static void StatusCode(HttpListenerResponse response, HttpStatusCode statusCode, object body = null)
        {
            if (response is null)
            {
                return;
            }

            response.AddCors();
            response.StatusCode = (int) statusCode;

            if (body is not null)
            {
                AddJsonOutput(response, body);
            }
        }

        public void Ok(HttpListenerResponse response, object body = null) => StatusCode(response, HttpStatusCode.OK, body); // 200

        public void BadRequest(HttpListenerResponse response, object body = null) => StatusCode(response, HttpStatusCode.BadRequest, body); // 400
        public void NotFound(HttpListenerResponse response, object body = null) => StatusCode(response, HttpStatusCode.NotFound, body); // 404

        public void InternalServerError(HttpListenerResponse response, object body = null) => StatusCode(response, HttpStatusCode.InternalServerError, body); // 500
    }
}
