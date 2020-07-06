using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbCountry : EntityBase
    {
        public long CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryCodeMua { get; set; }
        public string CountryNameTha { get; set; }
        public string CountryNameEng { get; set; }
        public string CountryShortNameTha { get; set; }
        public string CountryShortNameEng { get; set; }
        public bool Active { get; set; }
    }
}
