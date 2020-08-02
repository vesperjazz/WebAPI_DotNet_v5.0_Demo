using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using WebAPI_DotNetCore_Demo.Persistence;

namespace WebAPI_DotNetCore_Demo.Application.Tests
{
    public abstract class DbContextServiceTestBase : IDisposable
    {
        protected readonly string _databaseName;
        protected readonly WebAPIDemoDbContext _context;
        protected readonly Mock<ISystemClock> _mockSystemClock;
        protected readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        protected readonly UnitOfWork _unitOfWork;

        public DbContextServiceTestBase()
        {
            // Tests are made possible for applications that depends on EFCore directly
            // because of the existence of InMemory database, no physical database is needed.

            // 1. InMemory is not a relational database provider.
            // 2. Constraints won't be enforced, e.g. referential integrity, non-nullable columns and so on.
            // However, the whole point is to unit test your application business logic, not EFCore!
            // So it is okay for the InMemory database to have the limitations mentioned above.

            // Database name is a requirement for InMemoryDatabase since .NET Core 2.2.
            // The database name needs to be unique to guarantee a fresh InMemory database,
            // i.e. even if a new context is created with the same Database name, the same
            // InMemory database will be used across the tests, this behaviour is undesirable.
            _databaseName = Guid.NewGuid().ToString();
            var dbContextOptions = new DbContextOptionsBuilder<WebAPIDemoDbContext>()
                .UseInMemoryDatabase(databaseName: _databaseName)
                .Options;
            _context = new WebAPIDemoDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            // Leverage on existing transactional data initialiser to populate
            // the InMemory DbContext.
            DatabaseDataInitialiser.Initialise(_context);

            // Mocks are instantiated at the base for high reusability.
            // These instances can be setup individually in each test to fit
            // the context of the test.
            _mockSystemClock = new Mock<ISystemClock>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _unitOfWork = new UnitOfWork(_mockSystemClock.Object,
                _context, _mockHttpContextAccessor.Object);
        }

        public virtual void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
