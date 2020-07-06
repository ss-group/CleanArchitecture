using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuProgram : EntityBase
    {
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string ProgramPath { get; set; }
        public string SystemCode { get; set; }
        public string ModuleCode { get; set; }
        public ICollection<SuProgramLabel> ProgramLabels { get; set; }
    }
}
