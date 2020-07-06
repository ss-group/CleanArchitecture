using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbRegionConfiguration : BaseConfiguration<DbRegion>
    {
        public override void Configure(EntityTypeBuilder<DbRegion> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_region");
            builder.HasKey(e => e.RegionId);
        }
    }
}
