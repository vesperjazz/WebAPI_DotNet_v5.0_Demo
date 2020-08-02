using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Application.Exceptions
{
    public class DuplicateException : Exception, IException
    {
        public DuplicateException() { }

        public DuplicateException(string message) : base(message) { }

        public DuplicateException(string message, Exception inner) : base(message, inner) { }

        public async Task TransformHttpResponseAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await httpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(new
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = Message
                }));
        }
    }
}
