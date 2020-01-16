using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class SchemaElement
    {
        public string Name { get; private set; }
        public string MinOccurs { get; private set; }
        public string Type { get; private set; }
        public List<SchemaElement> SchemaElements { get; private set; } = new List<SchemaElement>();

        internal SchemaElement(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;

            if (node.ChildNodes.Count > 0)
            {
                node.ChildNodes[0].ChildNodes[0].ChildNodes.GetNodes()
                                                           .Where(n => n.Name.Contains("element"))
                                                           .ToList()
                                                           .ForEach(n =>
                                                           {
                                                               SchemaElements.Add(new SchemaElement(n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name") != null ? n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name").Value : string.Empty,
                                                                                                    n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs") != null ? n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs").Value : string.Empty,
                                                                                                    n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type") != null ? n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type").Value : string.Empty));
                                                           });
            }
        }

        internal SchemaElement(string name, string minOccurs, string type)
        {
            this.Name = name;
            this.MinOccurs = minOccurs;
            this.Type = type;
        }
    }
}
