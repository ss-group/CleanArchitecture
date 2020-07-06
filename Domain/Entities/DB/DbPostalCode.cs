using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbPostalCode : EntityBase
    {
        public long? PostalCodeId { get; set; }
        public string PostalCode { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? SubDistrictId { get; set; }
        public string PostalNameTha { get; set; }
        public string PostalNameEng { get; set; }
        public string Remark { get; set; }
        public bool Active { get; set; }
    }
}
