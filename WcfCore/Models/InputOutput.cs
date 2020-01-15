using System;
using System.Collections.Generic;
using System.Text;

namespace WcfCore.Models
{
    public class InputOutput
    {
        public string Action { get; private set; }
        public string Message { get; private set; }

        internal InputOutput(string action, string message)
        {
            this.Action = action;
            this.Message = message;
        }
    }
}
