using System;
using System.Collections;
using System.IO;
using CCSWE.nanoFramework.WebServer;
using CCSWE.nanoFramework.WebServer.Authorization;
using CCSWE.nanoFramework.WebServer.Http;

namespace Emily.Clock.Controllers;

[Route("/api/upload")]
[AllowAnonymous]
public class UploadController : ControllerBase
{
    // TODO: This implementation is very basic and does not handle many edge cases (e.g., multiple files, non-file sections, large files that exceed memory limits).
    // It also does not include any security measures (e.g., validating file types, preventing directory traversal attacks). A production implementation would need
    // to address these issues.
    [HttpPost]
    public void UploadFile()
    {
        if (!Request.IsMultipartContentType())
        {
            BadRequest("Invalid content type. Expected multipart/form-data.");
            return;
        }

        try
        {
            var boundary = Request.GetMultipartBoundary();
            if (string.IsNullOrEmpty(boundary))
            {
                BadRequest("Missing or invalid multipart boundary.");
                return;
            }

            var multipartReader = new MultipartReader(boundary, Request.Body);

            while (multipartReader.ReadNextSection() is { } section)
            {
                if (section.IsFileSection())
                {
                    var fileName = section.GetFileName();
                    if (string.IsNullOrEmpty(fileName))
                    {
                        BadRequest("File name is missing.");
                        return;
                    }

                    if (!Directory.Exists(@"D:\www"))
                    {
                        Directory.CreateDirectory(@"D:\www");
                    }
                    
                    var filePath = $@"D:\www\{fileName}";
                    using (var fileStream = File.OpenWrite(filePath))
                    {
                        var buffer = new byte[8192]; // Define a buffer size
                        int bytesRead;
                        while ((bytesRead = section.Body.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                        }
                    }


                    Ok($"File '{fileName}' uploaded successfully.");
                    return;
                }
            }

            BadRequest("No file found in the request.");
        }
        catch (Exception ex)
        {
            InternalServerError($"An error occurred: {ex.Message}");
        }
    }
}

// TODO: What is the non-file section and what will it take to support that?
public class MultipartSection
{
    public MemoryStream Body { get; set; }
    public Hashtable Headers { get; set; }

    public bool IsFileSection()
    {
        if (Headers == null || !Headers.Contains("Content-Disposition")) return false;

        var contentDisposition = Headers["Content-Disposition"] as string;
        if (string.IsNullOrEmpty(contentDisposition)) return false;

        // Check if the Content-Disposition header indicates a file
        return contentDisposition.Contains("filename=");
    }

    public string GetFileName()
    {
        if (Headers == null || !Headers.Contains("Content-Disposition")) return null;

        var contentDisposition = Headers["Content-Disposition"] as string;
        if (string.IsNullOrEmpty(contentDisposition)) return null;

        // Extract the filename from the Content-Disposition header
        var filenameIndex = contentDisposition.IndexOf("filename=");
        if (filenameIndex < 0) return null;

        var filenamePart = contentDisposition.Substring(filenameIndex + "filename=".Length).Trim();
        if (filenamePart.StartsWith("\"") && filenamePart.EndsWith("\"")) filenamePart = filenamePart.Substring(1, filenamePart.Length - 2);

        return filenamePart;
    }
}

public class MultipartReader
{
    public MultipartReader(string boundary, Stream requestBody)
    {
        if (string.IsNullOrEmpty(boundary))
        {
            throw new ArgumentException("Boundary cannot be null or empty.", nameof(boundary));
        }

        Boundary = boundary;
        RequestBody = requestBody ?? throw new ArgumentNullException(nameof(requestBody), "Request body cannot be null.");
    }

    public string Boundary { get; }

    public Stream RequestBody { get; }

    public MultipartSection? ReadNextSection()
    {
        // Ensure the request body is not null
        if (RequestBody == null)
        {
            throw new InvalidOperationException("Request body cannot be null.");
        }

        // Create a buffer to read the request body
        using var reader = new StreamReader(RequestBody, true);
        string line;
        var isBoundaryFound = false;

        // Read lines until the boundary is found
        while ((line = reader.ReadLine()) != null)
        {
            if (!line.StartsWith("--" + Boundary))
            {
                continue;
            }
            
            isBoundaryFound = true;
        }

        // If no boundary is found, return null
        if (!isBoundaryFound)
        {
            return null;
        }

        // Read headers
        var headers = new Hashtable();
        while (!string.IsNullOrEmpty(line = reader.ReadLine()))
        {
            var headerParts = line.Split([':'], 2);
            if (headerParts.Length == 2)
            {
                headers[headerParts[0].Trim()] = headerParts[1].Trim();
            }
        }

        // Read the body content
        var memoryStream = new MemoryStream();
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("--" + Boundary))
            {
                break;
            }

            var buffer = System.Text.Encoding.UTF8.GetBytes(line + "\r\n");
            memoryStream.Write(buffer, 0, buffer.Length);
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        // Return the multipart section
        return new MultipartSection { Headers = headers, Body = memoryStream };
    }
}

public static class HttpRequestExtensions 
{
    public static string? GetMultipartBoundary(this HttpRequest request)
    {
        var contentType = request.ContentType;

        if (string.IsNullOrEmpty(contentType))
        {
            return null;
        }

        // This really should be a case-insensitive check, but nanoFramework's string handling is limited and this is likely sufficient for most cases.
        var boundaryIndex = contentType.IndexOf("boundary=");
        if (boundaryIndex >= 0)
        {
            return contentType.Substring(boundaryIndex + "boundary=".Length).Trim('"');
        }
        
        return null;
    }
    
    public static bool IsMultipartContentType(this HttpRequest request)
    {
        if (request is null || string.IsNullOrEmpty(request.ContentType))
        {
            return false;
        }

        // This really should be a case-insensitive check, but nanoFramework's string handling is limited and this is likely sufficient for most cases.
        return request.ContentType.StartsWith("multipart/");
    }
}