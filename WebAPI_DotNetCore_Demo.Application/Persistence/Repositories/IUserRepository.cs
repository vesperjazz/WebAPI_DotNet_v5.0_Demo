using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Application.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByIDWithDetailsAsync(Guid userID, CancellationToken cancellationToken = default);
        Task<User> GetUserByUserNameWithDetailsAsync(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllUsersWithDetailsAsync(CancellationToken cancellationToken = default);
        void UpdateUserToInactive(User user);
    }
}
