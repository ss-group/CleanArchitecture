using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbSubProgram : EntityBase
    {
        public string CompanyCode { get; set; }
        public string MajorCode { get; set; }
        public string FacCode { get; set; }
        public string ProgramCode { get; set; }
        public string SubProgramCode { get; set; }
        public string SubProgramNameTha { get; set; }
        public string SubProgramNameEng { get; set; }
        public string SubProgramShortNameTha { get; set; }
        public string SubProgramShortNameEng { get; set; }
        public string FormatCode { get; set; }
        public bool Active { get; set; }
    }
}
