using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbFacConfiguration: BaseConfiguration<DbFac>
    {
        public override void Configure(EntityTypeBuilder<DbFac> builder)
        {
            base.Configure(builder);
            builder.HasKey(a => a.CompanyCode);
            builder.HasKey(e => e.FacCode);
        }
    }
}
