using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbPreName : EntityBase
    {
        public long PreNameId { get; set; }
        public string PreNameTha { get; set; }
        public string PreNameEng { get; set; }
        public string PreNameShortTha { get; set; }
        public string PreNameShortEng { get; set; }
        public string PreNameType { get; set; }
        public string PreNameCodeMua { get; set; }
        public bool? Active { get; set; }
    }
}
