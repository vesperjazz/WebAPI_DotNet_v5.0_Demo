using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Application.Persistence
{
    public interface IUnitOfWork : IRepositoryContainer, IDisposable
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
        Task<int> CompleteWithAuditAsync(CancellationToken cancellationToken = default);
    }
}
