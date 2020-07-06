using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuCompany : EntityBase
    {
        public string CompanyCode { get; set; }
        public string CompanyNameTha { get; set; }
        public string CompanyNameEng { get; set; }
        public string AddressTha { get; set; }
        public string AddressEng { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubDistrictId { get; set; }
        public int? PostalCodeId { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string PersonalTaxType { get; set; }
        public string TaxId { get; set; }
        public string SocailSecurityNo { get; set; }
        public string SocailSecurityBranch { get; set; }
        public string ReceiptBranchCode { get; set; }
        public string ReceiptBranchName { get; set; }
        public bool Active { get; set; }
        public ICollection<SuDivision> Divisions { get; set; }
        public ICollection<SuUserPermission> UserPermissions { get; set; }
        public ICollection<SuUserDivision> UserDivisions { get; set; }
    }
}
