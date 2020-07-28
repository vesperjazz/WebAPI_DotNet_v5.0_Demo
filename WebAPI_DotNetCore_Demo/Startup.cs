using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.MappingProfiles;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;
using WebAPI_DotNetCore_Demo.Application.Validators;
using WebAPI_DotNetCore_Demo.Authorizations;
using WebAPI_DotNetCore_Demo.Domain.Constants;
using WebAPI_DotNetCore_Demo.Extensions;
using WebAPI_DotNetCore_Demo.Middlewares;
using WebAPI_DotNetCore_Demo.Options;
using WebAPI_DotNetCore_Demo.Persistence;

namespace WebAPI_DotNetCore_Demo
{
    public class Startup
    {
        private const string AccessToken = "AccessToken";
        private const string JwtSettingsSection = "JwtSettings";
        private const string ConnectionStringSection = "WebAPIDemoDatabase";
        private const string SwaggerUISecurityName = "SwaggerUIJwt";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtSettings>(Configuration.GetSection(JwtSettingsSection));

            // To fix the following JsonException:
            // JsonException: A possible object cycle was detected which is not supported. 
            // This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
            services.AddControllers()
                // Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                // Install-Package FluentValidation.AspNetCore
                .AddFluentValidation(configuration => 
                {
                    configuration.ImplicitlyValidateChildProperties = true;
                    configuration.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ValidatorBase<>)));
                });

            // ServiceLifetime of DbContext is scoped by default, specifying to make the intentions clearer.
            // DbContext needs to be scoped as we want the container to return the same instance of DbContext
            // for all possible injections in the scope of each HttpRequest.
            services.AddDbContext<WebAPIDemoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(ConnectionStringSection)),
                ServiceLifetime.Scoped);

            // Similarly, the UnitOfWork is a wrapper for DbContext, so it should be scoped as well.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepositoryContainer>(x => x.GetService<IUnitOfWork>());

            // For auditing purposes in UnitOfWork.
            services.AddHttpContextAccessor();

            // Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
            // 1. Mapping defines the depth of serialisation, gets rid of the reference loop error above.
            // 2. Abstracts away the real database structure.
            // 3. Provides a more straight forward data transfer object required by each API.
            services.AddAutoMapper(Assembly.GetAssembly(typeof(ProfileBase)));

            // @TODO To implement a registration by assembly, i.e. no need for manual registration each
            // time a new service is added under the same assembly.
            // Transient items should be stateless and lightweight.
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<ILookupService, LookupService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<ISystemClock, SystemClock>();
            services.AddTransient<HMAC, HMACSHA512>();

            // Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtSettings = Configuration.GetSection(JwtSettingsSection).Get<JwtSettings>();
                    var issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuer = jwtSettings.IsValidateIssuer,
                        ValidateAudience = jwtSettings.IsValidateAudience,
                        IssuerSigningKey = issuerSigningKey
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (messageReceivedContext) =>
                        {
                            // Allows the AccessToken to be passed as query string parameter.
                            if (messageReceivedContext.Request.Query.ContainsKey(AccessToken))
                            {
                                messageReceivedContext.Token = messageReceivedContext.Request.Query[AccessToken];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.AdminOnly, authorizationPolicyBuilder =>
                {
                    authorizationPolicyBuilder.RequireRoleAuthorization(RoleConstants.Admin);
                });
            });

            // The beauty of using a custom IAuthorizationHandler is the access to dependency injection,
            // i.e. availability of data from the Database for more dynamic authorization requirements.
            // It is very important for the AuthorizationHandler to be scoped, as its dependency of
            // IRepositoryContainer is a scoped service as well.
            services.AddScoped<IAuthorizationHandler, WebAPIDemoAuthorizationHandler>();

            // @TODO Add custom WebAPI ProducesResponseType attribute for more accurate response types.
            // Currently only HttpStatusCode of 200 is available.
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "ASP.NET v3.1 WebAPI Demo";
                settings.Description = "SwaggerUI WebAPI Demo";
                settings.AddSecurity(SwaggerUISecurityName, new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization", // Key for Jwt header, cannot be any other value.
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor(SwaggerUISecurityName));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<SerilogMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
