using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;

namespace WebAPI_DotNetCore_Demo.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticatedUserDto> AuthenticateUserAsync(LoginUserDto loginUserDto,
            string secretKey, double expiryInMilliseconds, string issuer, string audience,
            CancellationToken cancellationToken = default);
        Task<UserDto> GetUserByIDAsync(Guid userID, CancellationToken cancellationToken = default);
        Task<UserDto> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        void SetUserInactive(Guid userID);
        Task CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    }
}
