using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Message
    {
        public string Name { get; private set; }
        public string Parameter { get; private set; }

        internal Message(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;
            if (node.ChildNodes.Count > 0)
            {
                this.Parameter = node.ChildNodes[0].Attributes.GetNodes().FirstOrDefault(n => n.Name == "element") != null ?
                                    node.ChildNodes[0].Attributes.GetNodes().FirstOrDefault(n => n.Name == "element").Value : string.Empty;
            }
        }
    }
}
