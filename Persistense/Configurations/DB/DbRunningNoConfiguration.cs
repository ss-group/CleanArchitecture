using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbRunningNoConfiguration :BaseConfiguration<DbRunningNo>
    {
        public override void Configure(EntityTypeBuilder<DbRunningNo> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.Id);
        }
    }
}
