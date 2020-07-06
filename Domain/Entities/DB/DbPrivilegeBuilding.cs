using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbPrivilegeBuilding : EntityBase
    {
        public long PrivilegeBuildingId { get; set; }
        public long BuildingId { get; set; }
        public string DivCode { get; set; }
        public bool Active { get; set; }
    }
}
