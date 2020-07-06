using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class UserProfileConfiguration : BaseConfiguration<SuUserProfile>
    {
        public override void Configure(EntityTypeBuilder<SuUserProfile> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Id).HasColumnName("user_id");
            builder.HasKey(e => new { e.Id, e.ProfileCode });
        }
    }
}
