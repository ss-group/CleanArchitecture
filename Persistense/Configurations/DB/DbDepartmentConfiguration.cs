using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbDepartmentConfiguration : BaseConfiguration<DbDepartment>
    {
        public override void Configure(EntityTypeBuilder<DbDepartment> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_department");
            builder.HasKey(e => e.DepartmentCode);
        }
    }
}
