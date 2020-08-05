using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Application.Exceptions
{
    public class SmtpConnectException : Exception, IException
    {
        public SmtpConnectException() { }

        public SmtpConnectException(string message) : base(message) { }

        public SmtpConnectException(string message, Exception inner) : base(message, inner) { }

        public async Task TransformHttpResponseAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(new
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = Message
                }));
        }
    }
}
