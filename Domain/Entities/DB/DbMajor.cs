using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbMajor : EntityBase
    {
        public string CompanyCode { get; set; }
        public string MajorCode { get; set; }
        public string FacCode { get; set; }
        public string ProgramCode { get; set; }
        public string MajorNameTha { get; set; }
        public string MajorNameEng { get; set; }
        public string MajorShortNameTha { get; set; }
        public string MajorShortNameEng { get; set; }
        public bool Active { get; set; }
        public string FormatCode { get; set; }
        public string MajorCodeMua { get; set; }

        public ICollection<DbProfessional> dbProfessional { get; set; }

    }
}
