using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
   public  class SuUserPermission : EntityBase
    {
        public long Id { get; set; }
        public string CompanyCode { get; set; }
        public ICollection<SuUserDivision>  Divisions {get;set;}
        public ICollection<SuUserEduLevel>  EduLevels { get; set; }
    }
}
