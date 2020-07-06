using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbFacprogramDetailConfiguration : BaseConfiguration<DbFacProgramDetail>
    {
        public override void Configure(EntityTypeBuilder<DbFacProgramDetail> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_fac_program_detail");
            builder.HasKey(e => new { e.CompanyCode, e.FacCode, e.ProgramCode, e.DepartmentCode });
        }
    }
}
