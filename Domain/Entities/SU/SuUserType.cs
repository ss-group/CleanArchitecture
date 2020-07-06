using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuUserType : EntityBase
    {
        public long UserId { get; set; }
        public string UserType { get; set; }
        public string CompanyCode { get; set; }
        public string EmployeeCode { get; set; }
        public long? StudentId { get; set; }
    }
}
