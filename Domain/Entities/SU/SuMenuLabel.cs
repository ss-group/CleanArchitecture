using Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuMenuLabel : EntityBase
    {
        public string SystemCode { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public Lang LangCode { get; set; }
    }
}
