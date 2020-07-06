using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbHoliday : EntityBase
    {
        public long? holidayId { get; set; }
        public string CompanyCode { get; set; }
        public long? year { get; set; }
        public DateTime? holidayDate { get; set; }
        public string holidayDesc { get; set; }
        public bool? active { get; set; }
    }
}
