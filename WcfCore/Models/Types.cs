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
            var imports = node.ChildNodes[0].ChildNodes.GetNodes().Where(ns => ns.Name.Contains("import")).ToList();

            node.ChildNodes[0].ChildNodes.GetNodes().Where(n => n.Name.Contains("import")).ToList().ForEach(n =>
            {
                Schemas.Add(new Schema(n.Attributes.GetNodes().FirstOrDefault(nn => nn.Name == "schemaLocation") != null ?
                                           n.Attributes.GetNodes().FirstOrDefault(nn => nn.Name == "schemaLocation").Value : string.Empty,
                                       n.Attributes.GetNodes().FirstOrDefault(nn => nn.Name == "namespace") != null ?
                                            n.Attributes.GetNodes().FirstOrDefault(nn => nn.Name == "namespace").Value : string.Empty,
                                       imports));
            });

            Schemas.Add(new Schema(node.ChildNodes[0]));
        }
    }
}
