using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbBuildingConfiguration : BaseConfiguration<DbBuilding>
    {
        public override void Configure(EntityTypeBuilder<DbBuilding> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_building");
            builder.HasKey(e => e.BuildingId);
            builder.HasMany(e => e.dbRoom).WithOne().HasForeignKey(p => p.BuildingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.dbPrivilegeBuilding).WithOne().HasForeignKey(p => p.BuildingId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
