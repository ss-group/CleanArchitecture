using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbSubDistrict : EntityBase
    {
        public long? SubDistrictId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictCodeMua { get; set; }
        public string SubDistrictNameTha { get; set; }
        public string SubDistrictNameEng { get; set; }
        public bool Active { get; set; }
    }
}
