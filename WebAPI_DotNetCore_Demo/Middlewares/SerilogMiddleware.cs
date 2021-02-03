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
        private const string MessageTemplate = "{RequestScheme} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext, IWebHostEnvironment env)
        {
            if (httpContext is null) { throw new ArgumentNullException(nameof(httpContext)); }

            var start = Stopwatch.GetTimestamp();
            var responseBodyStream = new MemoryStream();
            var originalResponseBodyStream = httpContext.Response.Body;
            var requestBody = await GetBodyAsync(httpContext.Request);

            try
            {
                httpContext.Response.Body = responseBodyStream;

                await _next(httpContext);

                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var responseBody = await GetBodyAsync(httpContext.Response);

                GetLoggerWithContext(httpContext, requestBody, responseBody, elapsedMs)
                    .Information(MessageTemplate, GetRequestScheme(httpContext),
                        httpContext.Request.Method, GetPath(httpContext),
                        httpContext.Response?.StatusCode, elapsedMs);

                await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            }
            catch (Exception ex)
            {
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                if (!env.IsDevelopment()) { await ProcessHttpResponseForException(httpContext, ex); }

                var responseBody = await GetBodyAsync(httpContext.Response);

                GetLoggerWithContext(httpContext, requestBody, responseBody, elapsedMs)
                    .Error(ex, MessageTemplate, GetRequestScheme(httpContext),
                        httpContext.Request.Method, GetPath(httpContext),
                        httpContext.Response?.StatusCode, elapsedMs);

                await responseBodyStream.CopyToAsync(originalResponseBodyStream);

                // Duplicate logs may appear in the log table due to rethrows.
                if (env.IsDevelopment()) { throw; }
            }
            finally
            {
                responseBodyStream.Dispose();
                httpContext.Response.Body = originalResponseBodyStream;
            }
        }

        private static async Task ProcessHttpResponseForException(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            // Open/Closed principle baby!
            if (exception is IException customException)
            {
                await customException.TransformHttpResponseAsync(httpContext);
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

        private static string GetRequestScheme(HttpContext httpContext)
        {
            return httpContext.Request.IsHttps ? "HTTPS" : "HTTP";
        }

        private static ILogger GetLoggerWithContext(HttpContext httpContext,
            string requestBody, string responseBody, double elapsedMs)
        {
            return Log.ForContext<SerilogMiddleware>()
                .ForContext("RequestMethod", httpContext.Request.Method)
                .ForContext("RequestPath", GetPath(httpContext))
                .ForContext("RequestBody", requestBody, true)
                .ForContext("ResponseStatusCode", httpContext.Response.StatusCode)
                .ForContext("ResponseBody", responseBody, true)
                .ForContext("ElapsedMs", elapsedMs)
                .ForContext("UserName", httpContext.User.Identity.Name);
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

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            _ = request.Body.Seek(0, SeekOrigin.Begin);

            return string.IsNullOrWhiteSpace(requestBody) ? null : requestBody;
        }

        private static async Task<string> GetBodyAsync(HttpResponse response)
        {
            _ = response.Body.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(response.Body, leaveOpen: true);
            var responseBody = await streamReader.ReadToEndAsync();

            _ = response.Body.Seek(0, SeekOrigin.Begin);

            return string.IsNullOrWhiteSpace(responseBody) ? null : responseBody;
        }
    }
}
