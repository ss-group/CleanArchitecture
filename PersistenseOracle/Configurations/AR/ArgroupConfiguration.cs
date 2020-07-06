using Domain.Entities.AR;
using Domain.Entities.CS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenseOracle.Configurations.AR
{
    public class ArgroupConfiguration : BaseOracleConfiguration<ArGroup>
    {
        public override void Configure(EntityTypeBuilder<ArGroup> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.ArgroupCode);
        }
    }
}
