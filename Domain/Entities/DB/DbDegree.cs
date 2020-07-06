using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbDegree : EntityBase
    {
        public long DegreeId { get; set; }
        public string DegreeNameTha { get; set; }
        public string DegreeNameEng { get; set; }
        public string DegreeShortNameTha { get; set; }
        public string DegreeShortNameEng { get; set; }
        public bool Active { get; set; }
    }
}
