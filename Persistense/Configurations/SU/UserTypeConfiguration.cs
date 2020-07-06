using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class UserTypeConfiguration : BaseConfiguration<SuUserType>
    {
        public override void Configure(EntityTypeBuilder<SuUserType> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.UserId);
        }
    }
}
