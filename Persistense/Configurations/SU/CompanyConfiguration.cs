using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class CompanyConfiguration : BaseConfiguration<SuCompany>
    {
        public override void Configure(EntityTypeBuilder<SuCompany> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompanyCode });
            builder.HasMany(e => e.Divisions).WithOne().HasForeignKey(f => f.CompanyCode).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.UserPermissions).WithOne().HasForeignKey(f => f.CompanyCode).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.UserDivisions).WithOne().HasForeignKey(f => f.CompanyCode).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
