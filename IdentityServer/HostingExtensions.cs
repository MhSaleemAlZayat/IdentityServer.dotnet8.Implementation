using IdentityServer.Data;
using IdentityServer.Data.Entities.AspIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        //Configure Asp Identity DbContext.
        builder.Services.AddDbContext<ApplicationIdentityDbContext>(
            options => options.UseSqlServer(connectionString)

        );

        builder.Services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
               .AddDefaultTokenProviders();

        // Configure IdentityServer with EF Core stores and ASP.NET Identity
        var isBuilder = builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            // this adds the config data from DB (clients, resources, CORS)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString,
                    dbOpts =>
                    {
                        dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName);
                        dbOpts.UseCompatibilityLevel(120);
                    });
            })
            // this is something you will want in production to reduce load on and requests to the DB
            .AddConfigurationStoreCache()
            //
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString,
                    dbOpts =>
                    {
                        dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName);
                        dbOpts.UseCompatibilityLevel(120);
                    }

                );

            })
            .AddAspNetIdentity<User>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.SeedingConfigurationData();
        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
