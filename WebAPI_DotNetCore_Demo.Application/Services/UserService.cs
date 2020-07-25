using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ISystemClock _systemClock;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRepositoryContainer _repositoryContainer;

        public UserService(IMapper mapper, ISystemClock systemClock, IPasswordService passwordService,
            IJwtTokenService jwtTokenService, IRepositoryContainer respositoryContainer)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _repositoryContainer = respositoryContainer ?? throw new ArgumentNullException(nameof(respositoryContainer));
        }

        public async Task<AuthenticatedUserDto> AuthenticateUserAsync(LoginUserDto loginUserDto,
            string secretKey, double expiryInMilliseconds, string issuer, string audience,
            CancellationToken cancellationToken = default)
        {
            var user = await _repositoryContainer.UserRepository.GetUserByUserNameWithDetailsAsync(loginUserDto.UserName, cancellationToken);

            if (user is null) { throw new NotFoundException($"User with UserName: [{loginUserDto.UserName}] is not found."); }

            var isPasswordCorrect = _passwordService.VerifyPassword(loginUserDto.Password, user.PasswordSalt, user.PasswordHash);

            if (!isPasswordCorrect) { throw new IncorrectPasswordException($"User with UserName: [{loginUserDto.UserName}] does not have Password: [{loginUserDto.Password}]"); }

            var expirationDate = _systemClock.UtcNow.AddMilliseconds(expiryInMilliseconds);

            var accessToken = _jwtTokenService.GetAccessToken(user.ID.Value, user.UserName,
                user.UserRoles.Select(ur => ur.Role.Name), issuer, audience, secretKey, expirationDate);

            var authenticateUserDto = new AuthenticatedUserDto
            {
                TokenExpiration = expirationDate.UtcDateTime,
                AccessToken = accessToken
            };

            return authenticateUserDto;
        }

        public async Task<UserDto> GetUserByIDAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryContainer.UserRepository.GetUserByIDWithDetailsAsync(userID, cancellationToken);

            if (user is null) { throw new NotFoundException($"User with ID: [{userID}] is not found."); }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryContainer.UserRepository.GetUserByUserNameWithDetailsAsync(userName, cancellationToken);

            if (user is null) { throw new NotFoundException($"User with username: [{userName}] is not found."); }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<UserDto>>(
                await _repositoryContainer.UserRepository.GetAllUsersWithDetailsAsync(cancellationToken));
        }

        public void SetUserInactive(Guid userID)
        {
            _repositoryContainer.UserRepository.UpdateUserToInactive(new User
            {
                ID = userID,
                IsActive = false
            });
        }

        public async Task CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
        {
            var newUser = _mapper.Map<User>(createUserDto);
            var (salt, hash) = _passwordService.CreatePasswordHash(createUserDto.Password);

            newUser.PasswordSalt = salt;
            newUser.PasswordHash = hash;

            await _repositoryContainer.UserRepository.AddAsync(newUser, cancellationToken);
        }
    }
}
