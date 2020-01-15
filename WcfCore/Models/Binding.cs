using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Binding
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Transport { get; private set; }
        public List<OperationBinding> OperationsBinding { get; private set; } = new List<OperationBinding>();

        internal Binding(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;
            this.Type = node.Attributes.GetNamedItem("type").Value;
            this.Transport = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("binding"))
                                 .Attributes.GetNamedItem("transport").Value;
            node.ChildNodes.GetNodes().Where(n => n.Name.Contains("operation"))
                                      .ToList()
                                      .ForEach(n =>
                                      {
                                          OperationsBinding.Add(new OperationBinding(n));
                                      });
        }
    }
}
