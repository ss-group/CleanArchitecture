using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class ParameterConfiguration : BaseConfiguration<SuParameter>
    {
        public override void Configure(EntityTypeBuilder<SuParameter> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ParameterGroupCode, e.ParameterCode });
        }
    }
}
