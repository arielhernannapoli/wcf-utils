using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Service
    {
        public string Name { get; private set; }
        public string PortName { get; private set; }
        public string PortBinding { get; private set; }
        public string AddressLocation { get; private set; }

        internal Service(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;
            this.PortName = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("port"))
                                 .Attributes.GetNamedItem("name").Value;
            this.PortBinding = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("port"))
                                 .Attributes.GetNamedItem("binding").Value;
            this.AddressLocation = node.ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("port"))
                                 .ChildNodes
                                 .GetNodes()
                                 .FirstOrDefault(n => n.Name.Contains("address"))
                                 .Attributes.GetNamedItem("location").Value;

        }
    }
}
