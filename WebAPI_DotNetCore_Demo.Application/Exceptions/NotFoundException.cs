using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Application.Exceptions
{
    public class NotFoundException : Exception, IException
    {
        public NotFoundException() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception inner) : base(message, inner) { }

        public async Task TransformHttpResponseAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

            await httpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(new
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    ErrorMessage = Message
                }));
        }
    }
}
