using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class UserPermissionConfiguration : BaseConfiguration<SuUserPermission>
    {
        public override void Configure(EntityTypeBuilder<SuUserPermission> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Id).HasColumnName("user_id");
            builder.HasKey(e => new { e.CompanyCode, e.Id });
            builder.HasMany(e=>e.Divisions).WithOne().HasForeignKey(p => new {p.CompanyCode, p.UserId});
            builder.HasMany(e => e.EduLevels).WithOne().HasForeignKey(p => new { p.CompanyCode, p.UserId });
        }
    }
}
