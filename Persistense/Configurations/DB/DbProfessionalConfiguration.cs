using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbProfessionalConfiguration : BaseConfiguration<DbProfessional>
    {
        public override void Configure(EntityTypeBuilder<DbProfessional> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_professional");
            builder.HasKey(e => new { e.CompanyCode, e.MajorCode, e.FacCode, e.ProgramCode, e.ProCode });
        }
    }
}
