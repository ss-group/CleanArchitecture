using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbDegreeConfiguration : BaseConfiguration<DbDegree>
    {
        public override void Configure(EntityTypeBuilder<DbDegree> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_degree");
            builder.HasKey(e => e.DegreeId);
        }
    }
}
