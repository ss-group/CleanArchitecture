using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbEducationType : EntityBase
    {
        public string CompanyCode { get; set; }
        public string EducationTypeCode { get; set; }
        public string EducationTypeNameTha { get; set; }
        public string EducationTypeNameEng { get; set; }
        public string EducationShortNameTha { get; set; }
        public string EducationShortNameEng { get; set; }
        public string TypeLevel { get; set; }
        public bool? Active { get; set; }
        public string FormatCode { get; set; }
        public string EducationTypeCodeMua { get; set; }
        public int? SummaryYear { get; set; }
        public int? NewLevelCode { get; set; }



    }
}
