using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbProvince : EntityBase
    {
        public long? ProvinceId { get; set; }
        public int? RegionId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceCodeMua { get; set; }
        public string ProvinceNameTha { get; set; }
        public string ProvinceNameEng { get; set; }
        public int? CountryId { get; set; }
        public string ProvinceShortNameTha { get; set; }
        public string ProvinceShortNameEng { get; set; }
        public bool Active { get; set; }
    }
}
