using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbProject : EntityBase
    {
        public long ProjectId { get; set; }
        public string CompanyCode { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectNameTha { get; set; }
        public string ProjectNameEng { get; set; }
        public string ProjectDescription { get; set; }
        public string EducationTypeCode { get; set; }
        public string FacCode { get; set; }
        public string StudentTypeCode { get; set; }
        public string ErpProduct { get; set; }
        public string ErpCode { get; set; }
        public string ErpActivity { get; set; }
        public string ErpProject { get; set; }
        public bool BypassRegister { get; set; }
        public bool Active { get; set; }
    }
}
