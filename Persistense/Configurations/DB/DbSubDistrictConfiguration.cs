using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.DB;

namespace Persistense.Configurations.DB
{
    class DbSubDistrictConfiguration
    {
        public class DbFacConfiguration : BaseConfiguration<DbSubDistrict>
        {
            public override void Configure(EntityTypeBuilder<DbSubDistrict> builder)
            {
                base.Configure(builder);
                builder.HasKey(a => a.SubDistrictId);
            }
        }
    }
}
