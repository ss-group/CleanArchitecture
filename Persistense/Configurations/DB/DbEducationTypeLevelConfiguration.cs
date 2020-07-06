using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbEducationTypeLevelConfiguration : BaseConfiguration<DbEducationTypeLevel>
    {
        public override void Configure(EntityTypeBuilder<DbEducationTypeLevel> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_education_type_level");
            builder.HasKey(e => new { e.CompanyCode, e.EducationTypeLevel});
        }
    }
}
