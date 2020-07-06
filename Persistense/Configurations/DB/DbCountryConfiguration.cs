using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbCountryConfiguration :BaseConfiguration<DbCountry>
    {
        public override void Configure(EntityTypeBuilder<DbCountry> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_country");
            builder.HasKey(e => e.CountryId);
        }
    }
}
