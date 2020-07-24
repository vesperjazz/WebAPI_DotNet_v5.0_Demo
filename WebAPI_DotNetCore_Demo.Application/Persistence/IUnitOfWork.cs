using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Application.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Gender> GenderRepository { get; }
        IRepository<Country> CountryRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IPersonRepository PersonRepository { get; }
        IUserRepository UserRepository { get; }

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
