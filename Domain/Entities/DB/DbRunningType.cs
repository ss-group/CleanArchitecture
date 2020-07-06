using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbRunningType : EntityBase
    {
        public int RunningTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RunningNoFormat { get; set; }
        public int RunningNoDigit { get; set; }
        public bool? Active { get; set; }
    }
}
