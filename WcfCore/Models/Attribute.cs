using System;
using System.Collections.Generic;
using System.Text;

namespace WcfCore.Models
{
    public class Attribute
    {
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Value { get; set; }

        internal Attribute(string prefix, string name, string ns, string value)
        {
            Prefix = prefix;
            Name = name;
            Namespace = ns;
            Value = value;
        }
    }
}
