using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbDegreeSubEduGroupConfiguration : BaseConfiguration<DbDegreeSubEduGroup>
    {
        public override void Configure(EntityTypeBuilder<DbDegreeSubEduGroup> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.SubDegreeId, e.GroupLevel });
        }
    }
}
