using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class PortType
    {
        public List<Operation> Operations { get; private set; } = new List<Operation>();

        internal PortType(XmlNode node)
        {
            node.ChildNodes.GetNodes().ForEach(n =>
            {
                Operations.Add(new Operation(n));
            });
        }
    }
}
