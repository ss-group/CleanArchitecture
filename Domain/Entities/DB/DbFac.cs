using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbFac: EntityBase
    {
        public string CompanyCode { get; set; }
        public string FacCode { get; set; }
        public string FacCodeMua { get; set; }
        public string FacNameTha { get; set; }
        public string FacNameEng { get; set; }
        public string FacShortNameTha { get; set; }
        public string FacShortNameEng { get; set; }
        public bool Active { get; set; }
        public string FormatCode { get; set; }
        public string FnPayinCode { get; set; }
    }
}
