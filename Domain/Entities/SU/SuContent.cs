using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuContent : EntityBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Reference { get; set; }
        
        public bool ValidatePath { get; set; }
    }
}
