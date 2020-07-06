using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbProfessional : EntityBase
    {
        public string CompanyCode { get; set; }
        public string MajorCode { get; set; }
        public string FacCode { get; set; }
        public string ProgramCode { get; set; }
        public string ProCode { get; set; }
        public string ProNameTha { get; set; }
        public string ProNameEng { get; set; }
        public string ProShortNameTha { get; set; }
        public string ProShortNameEng { get; set; }
        public string FormatCode { get; set; }
        public bool Active { get; set; }
    }
}
