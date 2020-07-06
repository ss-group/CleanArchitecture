using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
   public  class SuUserEduLevel : EntityBase
    {
        public long UserId { get; set; }
        public string CompanyCode { get; set; }
        public string EducationTypeLevel { get; set; }
    }
}
