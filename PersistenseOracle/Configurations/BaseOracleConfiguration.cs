using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenseOracle.Configurations
{
    public class BaseOracleConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBaseOracle
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CrBy).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(e => e.CrDate).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(e => e.ProgId).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
        }
    }
}
