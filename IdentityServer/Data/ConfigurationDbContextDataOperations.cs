using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityModel;
using IdentityServer.Data.Entities.AspIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer.Data;

public static class ConfigurationDbContextDataOperations
{
    private static List<Duende.IdentityServer.Models.Client> GetClients =>
        new List<Duende.IdentityServer.Models.Client>
        {
                // interactive ASP.NET Core Web App
                new Duende.IdentityServer.Models.Client
                {
                    ClientId = "web",
                    ClientSecrets = { new Duende.IdentityServer.Models.Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5003/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "verification",
                        "employee_api"
                    }
                }
        };


    private static List<Duende.IdentityServer.Models.IdentityResource> GetIdentityResources =>
         new List<Duende.IdentityServer.Models.IdentityResource>
    {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new Duende.IdentityServer.Models.IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }
    };

    public static List<Duende.IdentityServer.Models.ApiScope> GetApiScopes =>
       new List<Duende.IdentityServer.Models.ApiScope>
    {
            new Duende.IdentityServer.Models.ApiScope(name: "employee_api", displayName: "Employees API")
    };

    public static void SeedingConfigurationData(this WebApplication app)
    {
        try
        {
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                var userMnamager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                AddUser(userMnamager, "ahmed", "AhmedAli@email.com");
                AddUser(userMnamager, "samira", "SamiraMostafa@email.com");



                //Seeding Data

                if (!context.Clients.AnyAsync().Result)
                {
                    foreach (var client in GetClients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    var result = context.SaveChangesAsync().Result;
                }
                if (!context.IdentityResources.AnyAsync().Result)
                {
                    foreach (var resource in GetIdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    var result = context.SaveChangesAsync().Result;
                }
                if (!context.ApiScopes.AnyAsync().Result)
                {
                    foreach (var api in GetApiScopes)
                    {
                        context.ApiScopes.Add(api.ToEntity());
                    }
                    var result = context.SaveChangesAsync().Result;
                }

            }
        }
        catch (Exception exp)
        {

            throw exp;
        }
    }

    private static void AddUser(UserManager<User> userMnamager, string userName, string email)
    {
        string emailName = email.Split('@')[0];
        // Insert whitespace before each capital letter (except the first character)
        string emailNameWithSpaces = System.Text.RegularExpressions.Regex.Replace(emailName, "(?!^)([A-Z])", " $1");

        var ahmed = userMnamager.FindByNameAsync(userName).Result;
        if (ahmed == null)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };
            var result = userMnamager.CreateAsync(user, "P@ssw0rd").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            result = userMnamager.AddClaimsAsync(user, new[]
            {
                        new System.Security.Claims.Claim(JwtClaimTypes.Name, emailNameWithSpaces.TrimStart()),
                        new System.Security.Claims.Claim(JwtClaimTypes.GivenName, emailNameWithSpaces.TrimStart().Split(' ')[0]),
                        new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, emailNameWithSpaces.TrimStart().Split(' ')[1]),
                        new System.Security.Claims.Claim(JwtClaimTypes.WebSite, $"http://{userName}.com"),
                        new System.Security.Claims.Claim(JwtClaimTypes.Email, email) }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug($"User {emailNameWithSpaces} created");
        }
        else
        {
            Log.Debug($"User {userName} already exists");
        }
    }
}
