using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbProjectConfiguration : BaseConfiguration<DbProject>
    {
        public override void Configure(EntityTypeBuilder<DbProject> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_project");
            builder.HasKey(e => e.ProjectId);
        }
    }
}
