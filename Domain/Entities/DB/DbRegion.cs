using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbRegion : EntityBase
    {
        public long RegionId { get; set; }
        public long CountryId { get; set; }
        public string RegionCode { get; set; }
        public string RegionCodeMua { get; set; }
        public string RegionNameTha { get; set; }
        public string RegionNameEng { get; set; }
        public string RegionShortNameTha { get; set; }
        public string RegionShortNameEng { get; set; }
        public bool Active { get; set; }
    }
}
