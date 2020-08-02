using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.MappingProfiles;
using WebAPI_DotNetCore_Demo.Application.Services;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;
using Xunit;

namespace WebAPI_DotNetCore_Demo.Application.Tests
{
    public class UserServiceTests : DbContextServiceTestBase
    {
        private readonly Mapper _mapper;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IJwtTokenService> _mockjwtTokenService;
        private readonly UserService _userServiceSUT;

        public UserServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new UserProfileMapping())));
            _mockjwtTokenService = new Mock<IJwtTokenService>();
            _mockPasswordService = new Mock<IPasswordService>();
            _userServiceSUT = new UserService(_mapper, _mockSystemClock.Object,
                _mockPasswordService.Object, _mockjwtTokenService.Object, _unitOfWork);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        [Fact]
        public async Task AuthenticateUserAsync_InvalidUserName_ThrowsNotFoundException()
        {
            // Arrange
            var wrongUserName = "gandalf.greyhame";
            var loginUserDto = new LoginUserDto { UserName = wrongUserName };

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                // Act
                await _userServiceSUT.AuthenticateUserAsync(
                    loginUserDto, string.Empty, 0d, string.Empty, string.Empty);
            });
        }

        [Theory]
        [InlineData("wrongpassword")]
        [InlineData("blablawrongpassword")]
        public async Task AuthenticateUserAsync_WrongPassword_ThrowsIncorrectPasswordException(string wrongPassword)
        {
            var validUserName = "aragorn.elessar";
            var loginUserDto = new LoginUserDto { UserName = validUserName, Password = wrongPassword };

            _mockPasswordService
                .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            await Assert.ThrowsAsync<IncorrectPasswordException>(async () =>
            {
                await _userServiceSUT.AuthenticateUserAsync(
                    loginUserDto, string.Empty, 0d, string.Empty, string.Empty);
            });
        }

        [Theory]
        [InlineData("SuccessfulToken", 2020, 02, 29, 86_400_000)]
        [InlineData("AnotherSuccessfulToken", 2030, 12, 31, 172_800_000)]
        [InlineData("DoOrDoNotThereIsNoTry", 1991, 03, 13, 86_400_000)]
        public async Task AuthenticateUserAsync_SuccessfulLogin_ReturnsTokenAndExpiryDate(
            string expectedToken, int year, int month, int day, int expiryInMilliseconds)
        {
            // Arrange
            // @TODO To make use of TheoryData to enable strongly typed classes as arguments.
            var mockDateTime = new DateTimeOffset(new DateTime(year, month, day));
            var loginUserDto = new LoginUserDto { UserName = "aragorn.elessar", Password = "jrrtolkien" };

            _mockPasswordService
                .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);
            _mockSystemClock
                .Setup(x => x.UtcNow)
                .Returns(mockDateTime);
            _mockjwtTokenService
                .Setup(x => x.GetAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(expectedToken);

            // Act
            var authenticateUserDto = await _userServiceSUT.AuthenticateUserAsync(
                loginUserDto, string.Empty, expiryInMilliseconds, string.Empty, string.Empty);

            // Assert
            Assert.NotNull(authenticateUserDto);
            Assert.NotNull(authenticateUserDto.AccessToken);
            Assert.NotNull(authenticateUserDto.TokenExpiration);
            Assert.Equal(expectedToken, authenticateUserDto.AccessToken);
            Assert.Equal(mockDateTime.AddMilliseconds(expiryInMilliseconds).LocalDateTime, authenticateUserDto.TokenExpiration.Value);
        }

        [Theory]
        [InlineData("1817B6C9-AF73-4019-BFE2-08D8312EBC91")]
        [InlineData("38F7DB25-82D8-422E-9531-F93286FE4AF6")]
        public async Task GetUserByIDAsync_InvalidUserID_ThrowsNotFoundException(string invalidUserIDString)
        {
            // Arrange
            var invalidUserID = new Guid(invalidUserIDString);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                // Act
                await _userServiceSUT.GetUserByIDAsync(invalidUserID);
            });
        }

        [Theory]
        [InlineData("9338B511-C135-41A9-9ACE-48211DB19BE9")]
        [InlineData("30B801CC-216B-4F1B-6243-08D8312EBC95")]
        public async Task GetUserByIDAsync_ValidUserID_ReturnsUserDto(string validUserIDString)
        {
            // Arrange
            var validUserID = new Guid(validUserIDString);

            // Act
            var userDto = await _userServiceSUT.GetUserByIDAsync(validUserID);

            // Assert
            Assert.NotNull(userDto);
            Assert.NotEqual(Guid.Empty, userDto.ID);
            Assert.Equal(validUserID, userDto.ID);
            Assert.False(string.IsNullOrWhiteSpace(userDto.UserName));
            Assert.False(string.IsNullOrWhiteSpace(userDto.FirstName));
            Assert.False(string.IsNullOrWhiteSpace(userDto.LastName));
            Assert.NotEmpty(userDto.Roles);
        }

        [Theory]
        [InlineData("ivan.chin")]
        [InlineData("gandalf.greyhame")]
        public async Task GetUserByUserNameAsync_InvalidUserName_ThrowsNotFoundException(string userName)
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                // Act
                await _userServiceSUT.GetUserByUserNameAsync(userName);
            });
        }

        [Theory]
        [InlineData("aragorn.elessar")]
        [InlineData("arwen.undomiel")]
        public async Task GetUserByUserNameAsync_ValidUserName_ReturnsUserDto(string userName)
        {
            // Act
            var userDto = await _userServiceSUT.GetUserByUserNameAsync(userName);

            // Assert
            Assert.NotNull(userDto);
            Assert.NotEqual(Guid.Empty, userDto.ID);
            Assert.Equal(userName, userName);
            Assert.False(string.IsNullOrWhiteSpace(userDto.FirstName));
            Assert.False(string.IsNullOrWhiteSpace(userDto.LastName));
            Assert.NotEmpty(userDto.Roles);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUserDtos()
        {
            // Act
            var userDtos = await _userServiceSUT.GetAllUsersAsync();

            // Arrange
            Assert.NotNull(userDtos);
            Assert.NotEmpty(userDtos);
            Assert.Equal(2, userDtos.Count());
            Assert.All(userDtos, (userDto) =>
            {
                Assert.NotNull(userDto);
                Assert.NotEqual(Guid.Empty, userDto.ID);
                Assert.False(string.IsNullOrWhiteSpace(userDto.UserName));
                Assert.False(string.IsNullOrWhiteSpace(userDto.FirstName));
                Assert.False(string.IsNullOrWhiteSpace(userDto.LastName));
                Assert.NotEmpty(userDto.Roles);
            });
        }

        [Theory]
        [InlineData("aragorn.elessar")]
        [InlineData("arwen.undomiel")]
        public async Task CreateUserAsync_ExistingUser_ThrowsDuplicateException(string userName)
        {
            // Arrange
            var createUserDto = new CreateUserDto { UserName = userName };

            // Assert
            await Assert.ThrowsAsync<DuplicateException>(async () =>
            {
                // Act
                await _userServiceSUT.CreateUserAsync(createUserDto);
                await _unitOfWork.CompleteAsync();
            });
        }

        private const string ComplexPassword123Salt = "Xg27Yrq+6B2MVLn3RzKjkgNPRWHvQFFDYzoJ/fDiihkmhr7u+bYNht4Z5I+/zI1/SvwPoMwlddXXKzZvskhU4daWHDdxEExBNuvhlQPmgHz3Cky115iHrtcyKYLFq4SeSAXc5qflG/p2JKbv5VSA+dD5RBC5Dr4wLObcXSYnFis=";
        private const string ComplexPassword123Hash = "K+2ZX2Ld7ytvHzTCEn4PDUXKqS7RsH1mwhBlpfyxdeD2KYmag8T96Us98ppzrwXwrJFhQ4VSZxxVnxpxujuukA==";

        [Theory]
        [InlineData("gandalf.greyhame", "Gandalf", "Greyhame", "ComplexPassword123")]
        [InlineData("beric.dondarrion", "Beric", "Dondarrion", "ComplexPassword123")]
        [InlineData("daario.naharis", "Daario", "Naharis", "ComplexPassword123")]
        public async Task CreateUserAsync_NewUser_UserCreated(string userName, string firstName, string lastName, string password)
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };

            (byte[] Salt, byte[] Hash) expectedPasswordSaltHash = (Convert.FromBase64String(ComplexPassword123Salt), Convert.FromBase64String(ComplexPassword123Hash));
            _mockPasswordService
                .Setup(x => x.CreatePasswordHash(It.IsAny<string>()))
                .Returns(expectedPasswordSaltHash);

            // Act
            await _userServiceSUT.CreateUserAsync(createUserDto);
            await _unitOfWork.CompleteAsync();

            // Assert
            var domainUser = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == userName);

            Assert.NotNull(domainUser);
            Assert.NotNull(domainUser.ID);
            Assert.NotEqual(Guid.Empty, domainUser.ID);
            Assert.Equal(createUserDto.UserName, domainUser.UserName);
            Assert.Equal(createUserDto.FirstName, domainUser.FirstName);
            Assert.Equal(createUserDto.LastName, domainUser.LastName);
            Assert.Equal(expectedPasswordSaltHash.Salt, domainUser.PasswordSalt);
            Assert.Equal(expectedPasswordSaltHash.Hash, domainUser.PasswordHash);
            Assert.True(domainUser.IsActive);
        }
    }
}
