using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Application.Exceptions
{
    public interface IException
    {
        Task TransformHttpResponseAsync(HttpContext httpContext);
    }
}
