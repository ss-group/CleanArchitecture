using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbSubProgramConfiguration : BaseConfiguration<DbSubProgram>
    {
        public override void Configure(EntityTypeBuilder<DbSubProgram> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_sub_program");
            builder.HasKey(e => new { e.CompanyCode, e.MajorCode, e.FacCode, e.ProgramCode, e.SubProgramCode });
        }
    }
}
