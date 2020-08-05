using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Application.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> ConnectAsync(CancellationToken cancellationToken = default);

        Task<bool> SendEmailAsync(string subject, string body,
            string fromAddress, IEnumerable<string> toAddresses,
            IEnumerable<string> ccAddresses = default,
            IEnumerable<string> bccAddresses = default,
            CancellationToken cancellationToken = default);
    }
}
