using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbEducationTypeConfiguration : BaseConfiguration<DbEducationType>
    {
        
        public override void Configure(EntityTypeBuilder<DbEducationType> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_education_type");
            builder.HasKey(e => new { e.CompanyCode, e.EducationTypeCode });
        }
    }
}

