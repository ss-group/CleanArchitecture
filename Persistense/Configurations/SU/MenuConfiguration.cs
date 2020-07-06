using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class MenuConfiguration : BaseConfiguration<SuMenu>
    {
        public override void Configure(EntityTypeBuilder<SuMenu> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.MenuCode });
            builder.HasMany(e => e.SubMenus).WithOne().HasForeignKey(f => f.MainMenu).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.MenuLabels).WithOne().HasForeignKey(f => f.MenuCode).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
