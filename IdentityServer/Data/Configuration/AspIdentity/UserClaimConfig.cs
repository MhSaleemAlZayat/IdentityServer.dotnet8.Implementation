using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer.Data.Entities.AspIdentity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data.Configuration.AspIdentity
{
    public class UserClaimConfig : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");

        }
    }
}
