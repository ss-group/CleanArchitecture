using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbLocation : EntityBase
    {
        
        public string CompanyCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameTha { get; set; }
        public string LocationNameEng { get; set; }
        public bool Active { get; set; }
    }
}
