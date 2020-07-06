using Domain.Entities.DB;
using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class ContentConfiguration :BaseConfiguration<SuContent>
    {
        public override void Configure(EntityTypeBuilder<SuContent> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.Id);
        }
    }
}
