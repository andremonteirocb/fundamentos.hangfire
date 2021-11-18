using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundamentos.Hangfire
{
    public static class HangfireConfig
    {
        public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            //var mongoClient = new MongoClient(configuration.GetValue<string>("HangFire:ConnectionString"));

            services.AddHangfire(x => x
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMemoryStorage()
                    //.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                    //{
                    //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    //    QueuePollInterval = TimeSpan.FromSeconds(10),
                    //    UseRecommendedIsolationLevel = true,
                    //    JobExpirationCheckInterval = TimeSpan.FromMinutes(1),
                    //    UsePageLocksOnDequeue = true,
                    //    DisableGlobalLocks = true
                    //})
                    //.UseMongoStorage(mongoClient, configuration.GetValue<string>("HangFire:Database"), new MongoStorageOptions
                    //{
                    //    MigrationOptions = new MongoMigrationOptions
                    //    {
                    //        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    //        BackupStrategy = new CollectionMongoBackupStrategy()
                    //    },
                    //    Prefix = "hangfire.mongo",
                    //    CheckConnection = true
                    //})
                    );

            services.AddHangfireServer();
        }

        public static void UseHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                AppPath = null,
                DashboardTitle = "Hangfire Dashboard"
            });
        }

        public static void AddHangfireAuthorization(this IEndpointRouteBuilder endpoints, RequestDelegate requestDelegate)
        {
            endpoints.Map("/hangfire", requestDelegate).RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme });
        }
    }
}
