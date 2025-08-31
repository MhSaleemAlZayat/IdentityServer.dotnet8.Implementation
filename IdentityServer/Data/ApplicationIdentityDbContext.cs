using IdentityServer.Data.Configuration.AspIdentity;
using IdentityServer.Data.Entities.AspIdentity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public class ApplicationIdentityDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{

    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
    {


    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfig());
        builder.ApplyConfiguration(new RoleConfig());
        builder.ApplyConfiguration(new UserClaimConfig());
        builder.ApplyConfiguration(new UserRoleConfig());
        builder.ApplyConfiguration(new UserLoginConfig());
        builder.ApplyConfiguration(new RoleClaimConfig());
        builder.ApplyConfiguration(new UserTokenConfig());

    }
}
