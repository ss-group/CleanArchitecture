using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class UserEduLevelConfiguration : BaseConfiguration<SuUserEduLevel>
    {
        public override void Configure(EntityTypeBuilder<SuUserEduLevel> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompanyCode, e.UserId, e.EducationTypeLevel });
        }
    }
}
