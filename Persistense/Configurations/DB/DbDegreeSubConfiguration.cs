using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbDegreeSubConfiguration : BaseConfiguration<DbDegreeSub>
    {
        public override void Configure(EntityTypeBuilder<DbDegreeSub> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_degree_sub");
            builder.HasKey(e => e.SubDegreeId);
            builder.HasMany(e => e.dbDegreeSubEduGroup).WithOne().HasForeignKey(p =>  p.SubDegreeId ).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
