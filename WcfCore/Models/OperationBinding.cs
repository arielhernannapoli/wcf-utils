using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class OperationBinding
    {
        public string Name { get; private set; }
        public string Action { get; private set; }
        public string Style { get; private set; }
        public string InputUse { get; private set; }
        public string OutputUse { get; private set; }

        internal OperationBinding(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;
            this.Action = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("operation"))
                                 .Attributes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("Action"))
                                 .Value;
            this.Style = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("operation"))
                                 .Attributes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("style"))
                                 .Value;
            this.InputUse = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("input"))
                                 .ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("body"))
                                 .Attributes.GetNamedItem("use")
                                 .Value;
            this.OutputUse = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("output"))
                                 .ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("body"))
                                 .Attributes.GetNamedItem("use")
                                 .Value;
        }
    }
}
