using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer.Data.Entities.AspIdentity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data.Configuration.AspIdentity
{
    public class UserLoginConfig : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins");

        }
    }
}
