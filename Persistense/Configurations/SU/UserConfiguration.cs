using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class UserConfiguration : IEntityTypeConfiguration<SuUser>
    {
        public void Configure(EntityTypeBuilder<SuUser> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("user_id");
            builder.Ignore(p => p.TwoFactorEnabled);
            builder.Ignore(p => p.PhoneNumberConfirmed);
            builder.Ignore(p => p.NormalizedEmail);
            builder.Ignore(p => p.Email);
            builder.Ignore(p => p.EmailConfirmed);
            builder.Ignore(p => p.PhoneNumber);

            builder.HasMany(e => e.Profiles).WithOne().HasForeignKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.Permissions).WithOne().HasForeignKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.UserType).WithOne().HasForeignKey<SuUserType>(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(e => e.CreatedBy).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(e => e.CreatedDate).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(e => e.CreatedProgram).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(e => e.RowVersion).HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
        }
    }
}
