using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbProgramConfiguration: BaseConfiguration<DbProgram>
    {
        public override void Configure(EntityTypeBuilder<DbProgram> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_program");
            builder.HasKey(e => new { e.CompanyCode, e.ProgramCode});
           /* builder.HasMany(e => e.dbMajor).WithOne().HasForeignKey(p => new { p.CompanyCode, p.ProgramCode ,p.FacCode}).OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
