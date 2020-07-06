using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuUserProfile : EntityBase
    {
        public long Id { get; set; }
        public string ProfileCode { get; set; }
    }
}
