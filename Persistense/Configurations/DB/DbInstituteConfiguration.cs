using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    class DbInstituteConfiguration : BaseConfiguration<DbInstitute>
    {
        public override void Configure(EntityTypeBuilder<DbInstitute> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_institute");
            builder.HasKey(e => e.InstituteId);
        }
    }
}
