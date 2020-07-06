using Domain.Types;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuUser : IdentityUser<long>
    {
        public string PasswordPolicyCode { get; set; }
        public Lang DefaultLang { get; set; }
        public bool Active { get; set; }
        public bool ForceChangePassword { get; set; }
        public DateTime? StartEffectiveDate { get; set; }
        public DateTime? EndEffectiveDate { get; set; }
        public DateTime? LastChangePassword { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedProgram { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedProgram { get; set; }

        public uint? RowVersion { get; set; }

        public SuUserType UserType { get; set; }
        public ICollection<SuUserPermission> Permissions { get; set; }
        public ICollection<SuUserProfile> Profiles { get; set; }
    }
}
