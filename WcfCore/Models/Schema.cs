using System;
using System.Collections.Generic;
using System.Text;

namespace WcfCore.Models
{
    public class Schema
    {
        public string Location { get; private set; }
        public string Namespace { get; private set; }

        internal Schema(string location, string ns)
        {
            this.Location = location;
            this.Namespace = ns;
        }
    }
}
