using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbDegreeSub : EntityBase
    {
        public long SubDegreeId { get; set; }
        public long DegreeId { get; set; }
        public string SubDegreeNameTha { get; set; }
        public string SubDegreeNameEng { get; set; }
        public string SubDegreeShortNameTha { get; set; }
        public string SubDegreeShortNameEng { get; set; }
        public bool Active { get; set; }
        public ICollection<DbDegreeSubEduGroup> dbDegreeSubEduGroup { get; set; }
    }
}
