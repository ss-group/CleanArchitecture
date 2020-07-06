using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    class DbEmployeeConfiguration : BaseConfiguration<DbEmployee>
    {
        public override void Configure(EntityTypeBuilder<DbEmployee> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_employee");
            builder.HasKey(e => new { e.CompanyCode, e.EmployeeCode });
        }
    }
}
