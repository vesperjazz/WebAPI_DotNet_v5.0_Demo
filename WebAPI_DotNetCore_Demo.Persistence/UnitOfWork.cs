using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;
using WebAPI_DotNetCore_Demo.Persistence.Repositories;

namespace WebAPI_DotNetCore_Demo.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISystemClock _systemClock;
        private readonly WebAPIDemoDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

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

        public UnitOfWork(ISystemClock systemClock, WebAPIDemoDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CompleteWithAuditAsync(CancellationToken cancellationToken = default)
        {
            var currentDateTime = _systemClock.UtcNow.LocalDateTime;
            var currentUserID = new Guid(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
            var auditableEntries = _context.ChangeTracker.Entries<AuditEntityBase>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in auditableEntries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.Entity.CreateDate = currentDateTime;
                        entityEntry.Entity.CreateByUserID = currentUserID;
                        break;
                    case EntityState.Modified:
                        // This is necessary for operations that include _context.Set<T>().Update().
                        entityEntry.Property(e => e.CreateDate).IsModified = false;
                        entityEntry.Property(e => e.CreateByUserID).IsModified = false;
                        break;
                }
                entityEntry.Entity.UpdateDate = currentDateTime;
                entityEntry.Entity.UpdateByUserID = currentUserID;
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
