using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbPrivilegeBuildingConfiguration : BaseConfiguration<DbPrivilegeBuilding>
    {
        public override void Configure(EntityTypeBuilder<DbPrivilegeBuilding> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_privilege_building");
            builder.HasKey(e => e.PrivilegeBuildingId);
        }
    }
}
