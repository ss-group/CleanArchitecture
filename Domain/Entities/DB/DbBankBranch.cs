using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbBankBranch : EntityBase
    {
        public string BankCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchNameTh { get; set; }
        public string BranchNameEng { get; set; }
        public bool Active { get; set; }
    }
}
