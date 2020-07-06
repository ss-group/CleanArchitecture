using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class ErrorMessage
    {
        public string Code { get; set; }
        public IEnumerable<string> Parameters { get; private set; }

        public ErrorMessage(string messageCode)
        {
            this.Code = messageCode;
        }
        public ErrorMessage(string messageCode, params string[] parameters)
        {
            this.Code = messageCode;
            this.Parameters = parameters;
        }
    }
}
