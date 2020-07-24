using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Exceptions;

namespace WebAPI_DotNetCore_Demo.Middlewares
{
    public class SerilogMiddleware
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private string _requestBody;
        private string _responseBody;
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext, IWebHostEnvironment env)
        {
            if (httpContext is null) { throw new ArgumentNullException(nameof(httpContext)); }

            var start = Stopwatch.GetTimestamp();
            var responseBody = new MemoryStream();
            var originalResponseBodyStream = httpContext.Response.Body;

            try
            {
                _requestBody = await GetBodyAsync(httpContext.Request);
                // Replaces the content with null if empty or whitespace, so that null will be persisted into database.
                _requestBody = string.IsNullOrWhiteSpace(_requestBody) ? null : _requestBody;

                httpContext.Response.Body = responseBody;

                await _next(httpContext);

                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                _responseBody = await GetBodyAsync(httpContext.Response);
                _responseBody = string.IsNullOrWhiteSpace(_responseBody) ? null : _responseBody;

                GetLoggerWithContext(httpContext, elapsedMs, env.EnvironmentName).Information(MessageTemplate,
                    httpContext.Request.Method, GetPath(httpContext), httpContext.Response?.StatusCode, elapsedMs);

                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
            catch (Exception ex)
            {
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                if (!env.IsDevelopment()) { await ProcessHttpResponseForException(httpContext, ex); }

                _responseBody = await GetBodyAsync(httpContext.Response);
                _responseBody = string.IsNullOrWhiteSpace(_responseBody) ? null : _responseBody;

                GetLoggerWithContext(httpContext, elapsedMs, env.EnvironmentName).Error(ex, MessageTemplate,
                    httpContext.Request.Method, GetPath(httpContext), httpContext.Response?.StatusCode, elapsedMs);

                await responseBody.CopyToAsync(originalResponseBodyStream);

                // Duplicate logs may appear in the log table due to rethrows.
                if (env.IsDevelopment()) { throw; }
            }
            finally
            {
                responseBody.Dispose();
                httpContext.Response.Body = originalResponseBodyStream;
            }
        }

        private static async Task ProcessHttpResponseForException(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            // More custom exceptions to be handled in the future.
            // @TODO Add an injectable service to handle different types of 
            // exceptions and their different HttpStatusCode.
            if (exception is NotFoundException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

                await httpContext.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        ErrorMessage = exception.Message
                    }));
            }
            else if (exception is IncorrectPasswordException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await httpContext.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        HttpStatusCode = HttpStatusCode.Unauthorized,
                        ErrorMessage = exception.Message
                    }));
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await httpContext.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        HttpStatusCode = HttpStatusCode.InternalServerError,
                        ErrorMessage = exception.Message
                    }));
            }
        }

        private ILogger GetLoggerWithContext(HttpContext httpContext, double elapsedMs, string environmentName)
        {
            return Log.ForContext<SerilogMiddleware>()
                .ForContext("RequestMethod", httpContext.Request.Method)
                .ForContext("RequestPath", GetPath(httpContext))
                .ForContext("RequestBody", _requestBody, true)
                .ForContext("ResponseStatusCode", httpContext.Response.StatusCode)
                .ForContext("ResponseBody", _responseBody, true)
                .ForContext("ElapsedMs", elapsedMs)
                .ForContext("UserName", httpContext.User.Identity.Name)
                .ForContext("Environment", environmentName);
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }

        private static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
        }

        private static async Task<string> GetBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return bodyAsText;
        }

        private static async Task<string> GetBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var streamReader = new StreamReader(response.Body);
            var body = await streamReader.ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }
    }
}
