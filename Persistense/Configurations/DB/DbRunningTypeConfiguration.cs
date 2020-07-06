using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbRunningTypeConfiguration :BaseConfiguration<DbRunningType>
    {
        public override void Configure(EntityTypeBuilder<DbRunningType> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.RunningTypeId);
        }
    }
}
