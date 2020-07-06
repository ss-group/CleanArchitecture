using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbRunningNo : EntityBase
    {
        public int Id { get; set; }
        public int RunningTypeId { get; set; }
        public string RunningTypeCode { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }
        public string Parameter6 { get; set; }
        public string Parameter7 { get; set; }
        public string Parameter8 { get; set; }
        public string Parameter9 { get; set; }
        public string Parameter10 { get; set; }
        public int RunningNo { get; set; }
        public bool? Active { get; set; }
    }
}
