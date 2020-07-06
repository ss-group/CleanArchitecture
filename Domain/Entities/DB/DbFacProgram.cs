using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbFacProgram : EntityBase
    {
        public string CompanyCode { get; set; }
        public string FacCode { get; set; }
        public string ProgramCode { get; set; }
        public bool Active { get; set; }

        public ICollection<DbFacProgramDetail> dbFacProgramDetail { get; set; }

    }
}
