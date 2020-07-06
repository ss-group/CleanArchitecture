using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.DB;

namespace Persistense.Configurations.DB
{
    class DbProvinceConfiguration
    {
        public class DbFacConfiguration : BaseConfiguration<DbProvince>
        {
            public override void Configure(EntityTypeBuilder<DbProvince> builder)
            {
                base.Configure(builder);
                builder.HasKey(a => a.ProvinceId);
            }
        }
    }
}
