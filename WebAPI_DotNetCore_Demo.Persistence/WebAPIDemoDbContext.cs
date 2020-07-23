using Microsoft.EntityFrameworkCore;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;
using WebAPI_DotNetCore_Demo.Persistence.Extensions;

namespace WebAPI_DotNetCore_Demo.Persistence
{
    public class WebAPIDemoDbContext : DbContext
    {
        #region Lookups
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Country> Countries { get; set; }
        #endregion

        public DbSet<Person> Persons { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // https://github.com/serilog/serilog-sinks-mssqlserver
        // I have chosen to create the APILog table ahead of time, instead of allowing Serilog
        // to create it on the first run of the application.
        // This way, it is possible for table structure changes post production.
        // Note that this table does not follow the EntityBase convention, its ommission from the
        // UnitOfWork and Repository is by design, this table exist as part of the database, but
        // will be free from any manipulation or query programmatically.
        public DbSet<APILog> APILogs { get; set; }

        public WebAPIDemoDbContext(DbContextOptions<WebAPIDemoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}
