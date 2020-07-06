using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbPreNameConfiguration : BaseConfiguration<DbPreName>
    {
        public override void Configure(EntityTypeBuilder<DbPreName> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_pre_name");
            builder.HasKey(e => new { e.PreNameId });
        }
    }
}
