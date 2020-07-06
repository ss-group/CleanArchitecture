using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbBank : EntityBase
    {
        public string BankCode { get; set; }
        public string BankNameTh { get; set; }
        public string BankNameEng { get; set; }
        public bool? Active { get; set; }
    }
}
