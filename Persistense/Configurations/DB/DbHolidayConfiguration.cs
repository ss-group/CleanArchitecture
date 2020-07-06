using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbHolidayConfiguration : BaseConfiguration<DbHoliday>
    {
        public override void Configure(EntityTypeBuilder<DbHoliday> builder)
        {
            base.Configure(builder);
            //builder.ToTable("db_holiday");
            builder.HasKey(e => e.holidayId);
        }
    }
}
