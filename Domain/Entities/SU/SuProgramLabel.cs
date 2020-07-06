using Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuProgramLabel : EntityBase
    {
        public string ProgramCode { get; set; }
        public string FieldName { get; set; }
        public Lang LangCode { get; set; }
        public string LabelName { get; set; }
        public string SystemCode { get; set; }
        public string ModuleCode { get; set; }
    }
}
