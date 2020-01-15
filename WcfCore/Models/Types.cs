using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Types
    {
        public List<Schema> Schemas { get; private set; } = new List<Schema>();

        internal Types(XmlNode node)
        {
            node.ChildNodes[0].ChildNodes.GetNodes().ForEach(n =>
            {
                Schemas.Add(new Schema(n.Attributes.GetNamedItem("schemaLocation").Value,
                                       n.Attributes.GetNamedItem("namespace").Value));
            });
        }
    }
}
