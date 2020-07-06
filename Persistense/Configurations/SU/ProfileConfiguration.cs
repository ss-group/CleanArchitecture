using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class ProfileConfiguration : BaseConfiguration<SuProfile>
    {
        public override void Configure(EntityTypeBuilder<SuProfile> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ProfileCode });
            builder.HasMany(e => e.MenuProfiles).WithOne().HasForeignKey(f => f.ProfileCode).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
