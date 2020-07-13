using Microsoft.EntityFrameworkCore;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
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

        public WebAPIDemoDbContext(DbContextOptions<WebAPIDemoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}
