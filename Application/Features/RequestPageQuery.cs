using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public abstract class RequestPageQuery
    {
        public int Index { get; set; }
        private int _size;
        public int Size
        {
            get
            {
                return _size == 0 ? 10 : _size;
            }
            set
            {
                _size = value;
            }
        }
        public string Sort { get; set; }

    }
}
