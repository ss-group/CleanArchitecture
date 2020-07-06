using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbBuilding : EntityBase
    {
        public long BuildingId { get; set; }
        public string CompanyCode { get; set; }
        public string LocationCode { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingNameTha { get; set; }
        public string BuildingNameEng { get; set; }
        public bool Active { get; set; }
        public ICollection<DbRoom> dbRoom { get; set; }
        public ICollection<DbPrivilegeBuilding> dbPrivilegeBuilding { get; set; }
    }
}
