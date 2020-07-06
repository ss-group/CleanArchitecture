using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbRoom : EntityBase
    {
        public long RoomId { get; set; }
        public long BuildingId { get; set; }
        public string RoomNo { get; set; }
        public string RoomNameTha { get; set; }
        public string RoomNameEng { get; set; }
        public int FloorNo { get; set; }
        public int Capacity { get; set; }
        public string RoomType { get; set; }
        public bool Active { get; set; }
    }
}
