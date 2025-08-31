using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer.Data.Entities.AspIdentity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data.Configuration.AspIdentity
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

        }
    }
}
