using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbEmployee : EntityBase
    {
        public string CompanyCode { get; set; }
        public string EmployeeCode { get; set; }
        public string TeacherCode { get; set; }
        public string PersonalId { get; set; }
        public int? PreNameId { get; set; }
        public string tFirstName { get; set; }
        public string tMiddleName { get; set; }
        public string tLastName { get; set; }
        public string EmpTypeCode { get; set; }
        public string JobType { get; set; }
        public string PositionDivision { get; set; }
        public string PositionLevelCode { get; set; }
        public string PositionCode { get; set; }
        public string DivCode { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string AddrName { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Street { get; set; }
        public int? SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string TelNo { get; set; }
        public int? PostalCode { get; set; }
        public string Email { get; set; }
        public string RaceCode { get; set; }
        public string NationCode { get; set; }
        public string ReligionCode { get; set; }
        public DateTime? TurnoverDate { get; set; }
        public string eFirstName { get; set; }
        public string eMiddleName { get; set; }
        public string eLastName { get; set; }
        public string tNameConcat { get; set; }
        public string eNameConcat { get; set; }
        public string GroupTypeCode { get; set; }
        public long? ImageId { get; set; }
        public string divWorkId { get; set; }
    }
}
