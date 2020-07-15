using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Reflection;
using WebAPI_DotNetCore_Demo.Application.MappingProfiles;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;
using WebAPI_DotNetCore_Demo.Persistence;

namespace WebAPI_DotNetCore_Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // To fix the following JsonException -> Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson
            // JsonException: A possible object cycle was detected which is not supported. 
            // This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // ServiceLifetime of DbContext is scoped by default, specifying to make the intentions clearer.
            // DbContext needs to be scoped as we want the container to return the same instance of DbContext
            // for all possible injections in the scope of each HttpRequest.
            services.AddDbContext<WebAPIDemoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WebAPIDemoDatabase")),
                ServiceLifetime.Scoped);

            // Similarly, the UnitOfWork is a wrapper for DbContext, so it should be scoped as well.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
            // Mapping defines the depth of serialisation, gets rid of the reference loop error above.
            services.AddAutoMapper(Assembly.GetAssembly(typeof(ProfileBase)));

            // @TODO To implement a registration by assembly, i.e. no need for manual registration each
            // time a new service is added under the same assembly.
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<ILookupService, LookupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
