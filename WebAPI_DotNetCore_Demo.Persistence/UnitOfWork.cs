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
            _context.ChangeTracker.DetectChanges();

            var currentDateTime = _systemClock.UtcNow.UtcDateTime;
            var currentUserID = new Guid(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            var addedEntityEntries = _context.ChangeTracker.Entries()
                .Where(ee => ee.Entity is AuditEntityBase && ee.State == EntityState.Added);

            foreach (var entityEntry in addedEntityEntries)
            {
                var entity = entityEntry.Entity as AuditEntityBase;

                entity.CreateDate = currentDateTime;
                entity.CreateByUserID = currentUserID;
                entity.UpdateDate = currentDateTime;
                entity.UpdateByUserID = currentUserID;
            }

            var modifiedEntityEntries = _context.ChangeTracker.Entries()
                .Where(ee => ee.Entity is AuditEntityBase && ee.State == EntityState.Modified);

            foreach (var entityEntry in modifiedEntityEntries)
            {
                var entity = entityEntry.Entity as AuditEntityBase;

                entityEntry.Property("CreateDate").IsModified = false;
                entityEntry.Property("CreateByUserID").IsModified = false;
                entity.UpdateDate = currentDateTime;
                entity.UpdateByUserID = currentUserID;
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
