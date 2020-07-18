using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System;
using System.Collections.ObjectModel;

namespace WebAPI_DotNetCore_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Manual configuration is required at this point as the IConfiguration is
            // yet to be setup as seen in Startup.cs later in the application lifeline.
            // It is important to note that appsettings.json must have reloadOnChange set 
            // to true, else the changes that are done during runtime will not take effect
            // until next application restart.
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Both functionally equivalent, one using appsettings.json, another using code.
            ConfigureSerilogUsingAppSettings(configuration);
            //ConfigureSerilogUsingCode(configuration);

            try
            {
                // Since main is a static function, only static methods can be called.
                // There is no dependency injection at this point yet, so ILogger
                // (which will be configured as Serilog's logger) is not available
                // for use at this point.
                Log.Information("Starting application.");

                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureSerilogUsingAppSettings(IConfigurationRoot configuration)
        {
            // Install-Package Serilog.AspNetCore
            // Install-Package Serilog.Sinks.MSSqlServer
            // Install-Package Serilog.Enrichers.Environment
            // Install-Package Serilog.Enrichers.Process
            // Install-Package Serilog.Enrichers.Thread
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static void ConfigureSerilogUsingCode(IConfigurationRoot configuration)
        {
            var sinkOptions = new SinkOptions
            {
                TableName = "APILog",
                SchemaName = "dbo",
                AutoCreateSqlTable = false,
                BatchPostingLimit = 1000,
                BatchPeriod = TimeSpan.FromSeconds(5)
            };
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { ColumnName = "RequestMethod", DataType = System.Data.SqlDbType.NVarChar, DataLength = 10, AllowNull = true },
                    new SqlColumn { ColumnName = "RequestPath", DataType = System.Data.SqlDbType.NVarChar, DataLength = -1, AllowNull = true },
                    new SqlColumn { ColumnName = "RequestBody", DataType = System.Data.SqlDbType.NVarChar, DataLength = -1, AllowNull = true },
                    new SqlColumn { ColumnName = "ResponseStatusCode", DataType = System.Data.SqlDbType.Int, AllowNull = false },
                    new SqlColumn { ColumnName = "ResponseBody", DataType = System.Data.SqlDbType.NVarChar, DataLength = -1, AllowNull = true },
                    new SqlColumn { ColumnName = "ElapsedMs", DataType = System.Data.SqlDbType.Float, AllowNull = true },
                    new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100, AllowNull = true },
                    new SqlColumn { ColumnName = "MachineName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100, AllowNull = true },
                    new SqlColumn { ColumnName = "ProcessId", DataType = System.Data.SqlDbType.Int, AllowNull = false },
                    new SqlColumn { ColumnName = "ThreadId", DataType = System.Data.SqlDbType.Int, AllowNull = false }
                }
            };
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);

            // Install-Package Serilog.AspNetCore
            // Install-Package Serilog.Sinks.MSSqlServer
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName() // Install-Package Serilog.Enrichers.Environment
                .Enrich.WithProcessId()   // Install-Package Serilog.Enrichers.Process
                .Enrich.WithThreadId()    // Install-Package Serilog.Enrichers.Thread
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    "Server=IVANCHIN-PC;Database=WebAPIDemo;User Id=sa; Password=password",
                    sinkOptions: sinkOptions,
                    columnOptions: columnOptions)
                .CreateLogger();
        }

        // UseSerilog configures the HostBuilder to use Serilog as the logger
        // instead of the built-in .NET Core logger.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
