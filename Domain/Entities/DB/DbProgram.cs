using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbProgram: EntityBase
    {
        public string CompanyCode { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramNameTha { get; set; }
        public string ProgramNameEng { get; set; }
        public string ProgramShortNameTha { get; set; }
        public string ProgramShortNameEng { get; set; }
        /*public string FacCode { get; set; }*/
        public string FormatCode { get; set; }
        public string FnPayinCode { get; set; }
        public string SubjectGroupCode { get; set; }
        public string CourseDescriptionCode { get; set; }
        public bool Active { get; set; }
        public string CardName { get; set; }
        public bool Branch { get; set; }
        public string CurrId { get; set; }
        public string ProgramId { get; set; }
        public string IscedId { get; set; }
        public string RefGroupId { get; set; }
        public string ProgramCodeMua { get; set; }
        /*    public ICollection<DbMajor> dbMajor { get; set; }
            public ICollection<DbFac> dbFac { get; set; }*/

    }
}
