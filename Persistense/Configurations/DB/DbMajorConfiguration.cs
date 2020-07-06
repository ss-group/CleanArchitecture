using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbMajorConfiguration : BaseConfiguration<DbMajor>
    {
        public override void Configure(EntityTypeBuilder<DbMajor> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_major");
            builder.HasKey(e => new { e.CompanyCode, e.MajorCode, e.FacCode, e.ProgramCode });
            builder.HasMany(e => e.dbProfessional).WithOne().HasForeignKey(p => new { p.CompanyCode, p.MajorCode, p.FacCode, p.ProgramCode })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
