using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbBankBranchConfiguration : BaseConfiguration<DbBankBranch>
    {
        public override void Configure(EntityTypeBuilder<DbBankBranch> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_bank_branch");
            builder.HasKey(e => new { e.BankCode, e.BranchCode } );
        }
    }
}
