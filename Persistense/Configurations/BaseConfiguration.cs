using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations
{
    public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreatedBy).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(e => e.CreatedDate).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(e => e.CreatedProgram).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Ignore(e => e.Guid);
            builder.Ignore(e => e.RowState);
            builder.Property(e => e.RowVersion).HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

        }
    }
}
