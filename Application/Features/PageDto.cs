using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class PageDto
    {
        public IEnumerable<dynamic> Rows { get; set; }
        public long Count { get; set; }
    }
}
