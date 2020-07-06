using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class ProgramConfiguration : BaseConfiguration<SuProgram>
    {
        public override void Configure(EntityTypeBuilder<SuProgram> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.ProgramCode);
            builder.HasMany(e => e.ProgramLabels).WithOne().HasForeignKey(f => f.ProgramCode).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
