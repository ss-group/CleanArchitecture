using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class DivisionConfiguration : BaseConfiguration<SuDivision>
    {
        public override void Configure(EntityTypeBuilder<SuDivision> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompanyCode, e.DivCode });
            builder.HasMany(e => e.Divisions).WithOne().HasForeignKey(f => new { f.CompanyCode, f.DivParent }).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
