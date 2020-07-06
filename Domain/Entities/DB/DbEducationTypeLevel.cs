using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbEducationTypeLevel : EntityBase
    {
        public string CompanyCode { get; set; }
        public string EducationTypeLevel { get; set; }
        public string EducationTypeLevelNameTha { get; set; }
        public string EducationTypeLevelNameEng { get; set; }
        public string EducationLevelShortNameTha { get; set; }
        public string EducationLevelShortNameEng { get; set; }
        public string GraduatedNameTha { get; set; }
        public string GraduatedNameEng { get; set; }
        public bool? Active { get; set; }
        public string FormatCode { get; set; }
        public string GroupLevel { get; set; }
        public string Prefix { get; set; }
        public string EducationTypeCodeMua { get; set; }


    }
}
