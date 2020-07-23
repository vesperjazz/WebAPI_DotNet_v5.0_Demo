using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WebAPI_DotNetCore_Demo.Persistence;

namespace WebAPI_DotNetCore_Demo.Extensions
{
    public static class DatabaseExtensions
    {
        public static IHost InitialiseDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<WebAPIDemoDbContext>())
                {
                    Log.Information("Running pending database migrations.");

                    // Note the order in which the following methods are executed.
                    // Migrate will ensure latest migrations and seed data is inserted before
                    // the insertion of any dependent transactional data, e.g. User.

                    // Ensures all remaining migrations are applied on application run.
                    context.Database.Migrate();

                    Log.Information("Initialisting population of required transactional data.");

                    // Ensures the database is correctly populated with proper initial transactional data.
                    DatabaseDataInitialiser.Initialise(context);
                }
            }

            return host;
        }
    }
}
