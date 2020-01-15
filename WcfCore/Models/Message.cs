using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WcfCore.Models
{
    public class Message
    {
        public string Name { get; private set; }
        public string Parameter { get; private set; }

        internal Message(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;
            this.Parameter = node.ChildNodes[0].Attributes.GetNamedItem("element").Value;
        }
    }
}
