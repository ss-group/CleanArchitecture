using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class ProgramLabelConfiguration : BaseConfiguration<SuProgramLabel>
    {
        public override void Configure(EntityTypeBuilder<SuProgramLabel> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ProgramCode, e.FieldName, e.LangCode });
        }
    }
}
