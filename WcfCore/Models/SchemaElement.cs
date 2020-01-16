using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public string Namespace { get; private set; }

        public List<SchemaElement> SchemaElements { get; private set; } = new List<SchemaElement>();

        internal SchemaElement(XmlNode node, XmlNode parent, List<XmlNode> imports = null)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;

            if (node.ChildNodes.Count > 0)
            {
                if (node.ChildNodes[0].ChildNodes.Count > 0)
                {
                    node.ChildNodes[0].ChildNodes[0].ChildNodes.GetNodes()
                                                               .Where(n => n.Name.Contains("element"))
                                                               .ToList()
                                                               .ForEach(n =>
                                                               {
                                                                   var schemaElement = new SchemaElement(n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name") != null ? n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name").Value : string.Empty,
                                                                                                        n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs") != null ? n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs").Value : string.Empty,
                                                                                                        n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type") != null ? n.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type").Value : string.Empty);

                                                                   var typeName = schemaElement.Type.Contains(":") ?
                                                                                    schemaElement.Type.Split(':')[1] :
                                                                                    schemaElement.Type;

                                                                   schemaElement = AddSchemaElement(parent, typeName, schemaElement, imports);

                                                                   SchemaElements.Add(schemaElement);
                                                               });
                }
            }
        }

        internal SchemaElement(string name, string minOccurs, string type)
        {
            this.Name = name;
            this.MinOccurs = minOccurs;
            this.Type = type;
        }

        internal SchemaElement(string name, string minOccurs, string type, string ns)
        {
            this.Name = name;
            this.MinOccurs = minOccurs;
            this.Type = type;
            this.Namespace = ns;
        }

        internal SchemaElement AddSchemaElement(XmlNode parent, string typeName, SchemaElement schemaElement, List<XmlNode> imports)
        {
            if (parent.ChildNodes.GetNodes().FirstOrDefault(p => p.Attributes.GetNodes().FirstOrDefault(n => n.Name == "name") != null &&
                                                                       p.Attributes.GetNodes().FirstOrDefault(n => n.Name == "name").Value == typeName) != null)
            {
                var complexType = parent.ChildNodes.GetNodes().FirstOrDefault(p => p.Attributes.GetNamedItem("name").Value == typeName);

                if (complexType.ChildNodes.Count > 0)
                {
                    complexType.ChildNodes[0].ChildNodes.GetNodes()
                                                    .Where(nn => nn.Name.Contains("element"))
                                                    .ToList()
                                                    .ForEach(nn =>
                                                    {
                                                        var schemaElementChild = new SchemaElement(nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name") != null ? nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name").Value : string.Empty,
                                                                                             nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs") != null ? nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs").Value : string.Empty,
                                                                                             nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type") != null ? nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type").Value : string.Empty);

                                                        var typeNameChild = schemaElementChild.Type.Contains(":") ?
                                                                         schemaElementChild.Type.Split(':')[1] :
                                                                         schemaElementChild.Type;

                                                        schemaElementChild = AddSchemaElement(parent, typeNameChild, schemaElementChild, imports);

                                                        schemaElement.SchemaElements.Add(schemaElementChild);
                                                    });
                }
            }

            if(imports != null)
            {
                imports.ForEach(i =>
                {
                    var schemaLocation = i.Attributes.GetNamedItem("schemaLocation").Value;
                    var ns = i.Attributes.GetNamedItem("namespace").Value;

                    var httpClient = new HttpClient();
                    var httpResponse = httpClient.GetAsync(schemaLocation).Result;
                    var httpContent = httpResponse.Content.ReadAsStringAsync().Result;

                    var schemaDocument = new XmlDocument();
                    schemaDocument.LoadXml(httpContent);
                    
                    if (schemaDocument.DocumentElement.ChildNodes.GetNodes().FirstOrDefault(p => p.Attributes.GetNodes().FirstOrDefault(n => n.Name == "name") != null &&
                                                                               p.Attributes.GetNodes().FirstOrDefault(n => n.Name == "name").Value == typeName) != null)
                    {
                        var complexType = schemaDocument.DocumentElement.ChildNodes.GetNodes().FirstOrDefault(p => p.Attributes.GetNamedItem("name").Value == typeName);

                        if (complexType.ChildNodes.Count > 0)
                        {
                            complexType.ChildNodes[0].ChildNodes.GetNodes()
                                                            .Where(nn => nn.Name.Contains("element"))
                                                            .ToList()
                                                            .ForEach(nn =>
                                                            {
                                                                var schemaElementChild = new SchemaElement(nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name") != null ? nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "name").Value : string.Empty,
                                                                                                     nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs") != null ? nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "minOccurs").Value : string.Empty,
                                                                                                     nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type") != null ? nn.Attributes.GetNodes().FirstOrDefault(a => a.Name == "type").Value : string.Empty,
                                                                                                     ns);
                                                                schemaElement.SchemaElements.Add(schemaElementChild);
                                                            });
                        }
                    }
                });
            }
            
            return schemaElement;
        }
    }
}
