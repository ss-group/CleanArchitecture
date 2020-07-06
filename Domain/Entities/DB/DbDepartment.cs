using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbDepartment : EntityBase
    {
        public string DepartmentCode { get; set; }
        public string CompanyCode { get; set; }
        public string DepartmentCodeMua { get; set; }
        public string DepartmentNameTha { get; set; }
        public string DepartmentNameEng { get; set; }
        public string DepartmentShortNameTha { get; set; }
        public string DepartmentShortNameEng { get; set; }
        public bool Active { get; set; }
    }
}
