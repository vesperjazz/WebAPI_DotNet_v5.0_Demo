using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;
using WebAPI_DotNetCore_Demo.Persistence.Repositories;

namespace WebAPI_DotNetCore_Demo.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebAPIDemoDbContext _context;

        private Repository<Gender> _genderRepository;
        public IRepository<Gender> GenderRepository => _genderRepository ??= new Repository<Gender>(_context);

        private Repository<Country> _countryRepository;
        public IRepository<Country> CountryRepository => _countryRepository ??= new Repository<Country>(_context);

        private Repository<Role> _roleRepository;
        public IRepository<Role> RoleRepository => _roleRepository ??= new Repository<Role>(_context);

        private PersonRepository _personRepository;
        public IPersonRepository PersonRepository => _personRepository ??= new PersonRepository(_context);

        private UserRepository _userRepository;
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public UnitOfWork(WebAPIDemoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
