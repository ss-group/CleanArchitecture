using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbFacProgramDetail : EntityBase
    {
        public string CompanyCode { get; set; }
        public string FacCode { get; set; }
        public string ProgramCode { get; set; }
        public string DepartmentCode { get; set; }
        public bool Active { get; set; }

    }
}
