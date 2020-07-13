using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;

namespace WebAPI_DotNetCore_Demo.Application.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Gender> GenderRepository { get; }
        IRepository<Country> CountryRepository { get; }
        IPersonRepository PersonRepository { get; }

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
