using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbLocationConfiguration : BaseConfiguration<DbLocation>
    {
        public override void Configure(EntityTypeBuilder<DbLocation> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_location");
            builder.HasKey(e => new { e.CompanyCode, e.LocationCode});
        }
    }
}
