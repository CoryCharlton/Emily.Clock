using System.IO;
using nanoFramework.Json;
using System.Net;
using System.Text;
using Emily.Clock.Http;
using MakoIoT.Device.Services.Server.Extensions;

#nullable enable // TODO: Enable this for the project
namespace Emily.Clock.Controllers
{
    public abstract class ControllerBase
    {
        private const int BufferSize = 1024;

        private static void WriteOutput(HttpListenerResponse response, object? body)
        {
            var bytes = Encoding.UTF8.GetBytes(body as string ?? JsonConvert.SerializeObject(body));

            WriteOutput(response, bytes);
        }

        private static void WriteOutput(HttpListenerResponse response, byte[] bytes, string contentType = MimeType.Application.Json)
        {
            response.ContentLength64 = bytes.Length;
            response.ContentType = contentType;
            response.SendChunked = response.ContentLength64 > BufferSize; // Is this good?

            for (var bytesSent = 0L; bytesSent < bytes.Length;)
            {
                var bytesToSend = bytes.Length - bytesSent;
                bytesToSend = bytesToSend < BufferSize ? bytesToSend : BufferSize;

                response.OutputStream.Write(bytes, (int) bytesSent, (int) bytesToSend);
                bytesSent += bytesToSend;
            }
        }

        private static void WriteOutput(HttpListenerResponse response, Stream stream, string contentType = MimeType.Application.Octet)
        {
            response.ContentLength64 = stream.Length;
            response.ContentType = contentType;
            response.SendChunked = response.ContentLength64 > BufferSize; // Is this good?

            var buffer = new byte[BufferSize];
            var bytesSent = 0L;
            int bytesToSend;

            while ((bytesToSend = stream.Read(buffer)) > 0)
            {
                response.OutputStream.Write(buffer, (int) bytesSent, (int) bytesToSend);
                bytesSent += bytesToSend;
            }
        }

        protected static void StatusCode(HttpListenerResponse response, HttpStatusCode statusCode, object? body = null, string contentType = MimeType.Application.Json)
        {
            response.AddCors();
            response.StatusCode = (int) statusCode;

            switch (body)
            {
                case byte[] bytes:
                    WriteOutput(response, bytes, contentType);
                    break;
                case Stream stream:
                    WriteOutput(response, stream);
                    break;
                default:
                {
                    if (body is not null)
                    {
                        WriteOutput(response, body);
                    }

                    break;
                }
            }
        }

        protected void Ok(HttpListenerResponse response, object? body = null, string contentType = MimeType.Application.Json) => StatusCode(response, HttpStatusCode.OK, body, contentType); // 200

        protected void BadRequest(HttpListenerResponse response, object? body = null, string contentType = MimeType.Application.Json) => StatusCode(response, HttpStatusCode.BadRequest, body, contentType); // 400
        protected void NotFound(HttpListenerResponse response, object? body = null, string contentType = MimeType.Application.Json) => StatusCode(response, HttpStatusCode.NotFound, body, contentType); // 404

        protected void InternalServerError(HttpListenerResponse response, object? body = null, string contentType = MimeType.Application.Json) => StatusCode(response, HttpStatusCode.InternalServerError, body, contentType); // 500
    }
}
