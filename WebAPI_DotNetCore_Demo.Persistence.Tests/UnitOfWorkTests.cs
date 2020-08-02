using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using Xunit;

namespace WebAPI_DotNetCore_Demo.Persistence.Tests
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly string _databaseName;
        private readonly WebAPIDemoDbContext _context;
        private readonly Mock<ISystemClock> _mockSystemClock;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly UnitOfWork _unitOfWorkSUT;

        public UnitOfWorkTests()
        {
            _databaseName = Guid.NewGuid().ToString();
            var dbContextOptions = new DbContextOptionsBuilder<WebAPIDemoDbContext>()
                // Install-Package Microsoft.EntityFrameworkCore.InMemory
                .UseInMemoryDatabase(databaseName: _databaseName)
                .Options;
            _context = new WebAPIDemoDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            DatabaseDataInitialiser.Initialise(_context);

            // Refer to comments in DbContextServiceTestBase.cs
            _mockSystemClock = new Mock<ISystemClock>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _unitOfWorkSUT = new UnitOfWork(_mockSystemClock.Object,
                _context, _mockHttpContextAccessor.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Theory]
        [InlineData("9338B511-C135-41A9-9ACE-48211DB19BE9", 1991, 03, 14)]
        [InlineData("30B801CC-216B-4F1B-6243-08D8312EBC95", 2030, 06, 21)]
        public async Task CompleteWithAuditAsync_CreateAuditableItems_CorrectAuditFieldsPopulated(
            string existingUserIDString, int year, int month, int day)
        {
            // Arrange
            var existingUserID = new Guid(existingUserIDString);
            var mockDateTime = new DateTimeOffset(new DateTime(year, month, day));
            var mockHttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(new List<Claim> 
                    { 
                        new Claim(ClaimTypes.NameIdentifier, existingUserIDString),
                    })
                })
            };
            _mockSystemClock
                .Setup(x => x.UtcNow)
                .Returns(mockDateTime);
            _mockHttpContextAccessor.Setup(x => x.HttpContext)
                .Returns(mockHttpContext);

            // For ease of query later.
            var newPersonID = Guid.NewGuid();
            var newPerson = new Person { ID = newPersonID };
            
            // Act
            await _unitOfWorkSUT.PersonRepository.AddAsync(newPerson);
            await _unitOfWorkSUT.CompleteWithAuditAsync();

            // Assert
            var domainPerson = await _context.Persons.SingleOrDefaultAsync(p => p.ID == newPersonID);

            Assert.NotNull(domainPerson);
            Assert.NotNull(domainPerson.CreateDate);
            Assert.NotNull(domainPerson.CreateByUserID);
            Assert.NotNull(domainPerson.UpdateDate);
            Assert.NotNull(domainPerson.UpdateByUserID);

            Assert.Equal(mockDateTime.LocalDateTime, domainPerson.CreateDate);
            Assert.Equal(existingUserID, domainPerson.CreateByUserID);
            Assert.Equal(mockDateTime.LocalDateTime, domainPerson.UpdateDate);
            Assert.Equal(existingUserID, domainPerson.UpdateByUserID);
        }

        [Theory]
        [InlineData("B3568499-BD4C-4BF6-A7CA-EF35EEC2DC74", "9338B511-C135-41A9-9ACE-48211DB19BE9", 2022, 12, 25)]
        [InlineData("C9B56586-BF47-47ED-B07F-34AF1FE854AC", "30B801CC-216B-4F1B-6243-08D8312EBC95", 2030, 03, 14)]
        public async Task CompleteWithAuditAsync_UpdateAuditableItems_CorrectAuditFieldsPopulated(
            string updateUserIDString, string existingUserIDString, int year, int month, int day)
        {
            // Arrange
            // Note that updateUserID is something that does not exist in the InMemory database.
            // Referential integrity is not enforced, so it works in our favour that the user with
            // ID = updateUserID need not be created prior to testing an update scenario.
            var updateUserID = new Guid(updateUserIDString);
            var existingUserID = new Guid(existingUserIDString);
            var mockUpdateDateTime = new DateTimeOffset(new DateTime(year, month, day));
            var mockHttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, updateUserIDString),
                    })
                })
            };
            _mockSystemClock
                .Setup(x => x.UtcNow)
                .Returns(mockUpdateDateTime);
            _mockHttpContextAccessor.Setup(x => x.HttpContext)
                .Returns(mockHttpContext);

            var domainUser = await _context.Users
                .SingleOrDefaultAsync(u => u.ID == existingUserID);

            // Edit the tracked entity, any field will do.
            domainUser.FirstName = "Frodo";
            domainUser.LastName = "Baggins";

            // Act
            await _unitOfWorkSUT.CompleteWithAuditAsync();

            var updatedDomainUser = await _context.Users
                .SingleOrDefaultAsync(u => u.ID == existingUserID);

            Assert.NotNull(updatedDomainUser);
            Assert.NotNull(updatedDomainUser.CreateDate);
            Assert.NotNull(updatedDomainUser.CreateByUserID);
            Assert.NotNull(updatedDomainUser.UpdateDate);
            Assert.NotNull(updatedDomainUser.UpdateByUserID);

            Assert.NotEqual(mockUpdateDateTime.LocalDateTime, updatedDomainUser.CreateDate);
            Assert.NotEqual(updateUserID, updatedDomainUser.CreateByUserID);

            Assert.Equal(mockUpdateDateTime.LocalDateTime, updatedDomainUser.UpdateDate);
            Assert.Equal(updateUserID, updatedDomainUser.UpdateByUserID);
        }
    }
}
