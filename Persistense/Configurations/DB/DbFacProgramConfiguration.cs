using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbFacProgramConfiguration : BaseConfiguration<DbFacProgram>
    {
        public override void Configure(EntityTypeBuilder<DbFacProgram> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_fac_program");
            builder.HasKey(e => new { e.CompanyCode, e.FacCode, e.ProgramCode });
            builder.HasMany(e => e.dbFacProgramDetail).WithOne().HasForeignKey(p => new { p.CompanyCode, p.FacCode, p.ProgramCode })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
