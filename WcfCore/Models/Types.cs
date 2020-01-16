using System;
using System.Collections.Generic;
using System.Linq;
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
            node.ChildNodes[0].ChildNodes.GetNodes().Where(n => n.Name.Contains("import")).ToList().ForEach(n =>
            {
                Schemas.Add(new Schema(n.Attributes.GetNamedItem("schemaLocation").Value,
                                       n.Attributes.GetNamedItem("namespace").Value));
            });

            Schemas.Add(new Schema(node.ChildNodes[0]));
        }
    }
}
