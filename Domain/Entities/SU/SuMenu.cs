using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuMenu : EntityBase
    {
        public string MenuCode { get; set; }
        public string SystemCode { get; set; }
        public string MainMenu { get; set; }
        public string ProgramCode { get; set; }
        public string Icon { get; set; }
        public bool Active { get; set; }
        public ICollection<SuMenu> SubMenus { get; set; }
        public ICollection<SuMenuLabel> MenuLabels { get; set; }
    }
}
