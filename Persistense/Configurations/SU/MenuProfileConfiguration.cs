using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class MenuProfileConfiguration : BaseConfiguration<SuMenuProfile>
    {
        public override void Configure(EntityTypeBuilder<SuMenuProfile> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ProfileCode, e.MenuCode });
        }
    }
}
