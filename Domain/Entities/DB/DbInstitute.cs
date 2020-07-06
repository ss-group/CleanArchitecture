using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbInstitute : EntityBase
    {
        public long InstituteId { get; set; }
        public string InstituteNameTha { get; set; }
        public string InstituteNameEng { get; set; }
        public string InstituteShortNameTha { get; set; }
        public string InstituteShortNameEng { get; set; }
        public string AddrFull { get; set; }
        public string InstituteType { get; set; }
        public string ContactName { get; set; }
        public string AddressName { get; set; }
        public string AddressNo { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public int? SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public int? PostalId { get; set; }
        public int? CountryId { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool Active { get; set; }
        public long? VerifyInstituteId { get; set; }
        
    }
}
