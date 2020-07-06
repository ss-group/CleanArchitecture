using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuPasswordPolicy : EntityBase
    {
        public string PasswordPolicyCode { get; set; }
        public string PasswordPolicyName { get; set; }
        public string PasswordPolicyDescription { get; set; }
        public int? PasswordMinimumLength { get; set; }
        public int? PasswordMaximumLength { get; set; }
        public int? FailTime { get; set; }
        public int? PasswordAge { get; set; }
        public int? MaxDupPassword { get; set; }
        public bool UsingUppercase { get; set; }
        public bool UsingLowercase { get; set; }
        public bool UsingNumericChar { get; set; }
        public bool UsingSpecialChar { get; set; }
        public bool Active { get; set; }
    }
}
