using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbListItemConfiguration  : BaseConfiguration<DbListItem>
    {
        public override void Configure(EntityTypeBuilder<DbListItem> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.ListItemGroupCode);
            builder.HasKey(a => a.ListItemCode);
        }
 
    }
}
