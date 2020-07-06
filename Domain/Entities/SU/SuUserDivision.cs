using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
   public  class SuUserDivision : EntityBase
    {
        public long UserId { get; set; }
        public string CompanyCode { get; set; }
        public string DivCode { get; set; }
    }
}
