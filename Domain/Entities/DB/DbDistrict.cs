using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbDistrict : EntityBase
    {
        public long? DistrictId { get; set; }
        public long? ProvinceId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictCodeMua { get; set; }
        public string DistrictNameTha { get; set; }
        public string DistrictNameEng { get; set; }
        public bool Active { get; set; }
    }
}
