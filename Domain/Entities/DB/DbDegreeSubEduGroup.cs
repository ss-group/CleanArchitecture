using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbDegreeSubEduGroup : EntityBase
    {
        public long? SubDegreeId { get; set; }
        public string GroupLevel { get; set; }
        public bool Active { get; set; }

    }
}
