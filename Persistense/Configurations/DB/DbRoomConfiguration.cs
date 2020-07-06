using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbRoomConfiguration : BaseConfiguration<DbRoom>
    {
        public override void Configure(EntityTypeBuilder<DbRoom> builder)
        {
            base.Configure(builder);
            builder.ToTable("db_room");
            builder.HasKey(e => e.RoomId);
        }
    }
}
