using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly WebAPIDemoDbContext _context;
        public UserRepository(WebAPIDemoDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private IQueryable<User> QueryUserWithDetails()
        {
            return _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
        }

        public async Task<IEnumerable<User>> GetAllUsersWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await QueryUserWithDetails().ToListAsync(cancellationToken);
        }

        public async Task<User> GetUserByIDWithDetailsAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            return await QueryUserWithDetails().SingleOrDefaultAsync(u => u.ID == userID, cancellationToken);
        }

        public async Task<User> GetUserByUserNameWithDetailsAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await QueryUserWithDetails().SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public void UpdateUserToInactive(User user)
        {
            var userEntityEntry = _context.Users.Attach(user);
            userEntityEntry.Property(p => p.IsActive).IsModified = true;
        }
    }
}
