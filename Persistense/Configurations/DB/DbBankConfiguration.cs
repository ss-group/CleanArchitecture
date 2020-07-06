using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbBankConfiguration : BaseConfiguration<DbBank>

    {
        public override void Configure(EntityTypeBuilder<DbBank> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_bank");
            builder.HasKey(e => e.BankCode);
        }
    }
}
