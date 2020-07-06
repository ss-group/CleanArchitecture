using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuProfile : EntityBase
    {
        public string ProfileCode { get; set; }
        public string ProfileDesc { get; set; }
        public bool? Active { get; set; }
        public ICollection<SuMenuProfile> MenuProfiles { get; set; }
    }
}
