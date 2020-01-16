using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Schema
    {
        public string Location { get; private set; }
        public string Namespace { get; private set; }
        public List<SchemaElement> SchemaElements { get; private set; } = new List<SchemaElement>();

        internal Schema(string location, string ns)
        {
            this.Location = location;
            this.Namespace = ns;

            var httpClient = new HttpClient();
            var httpResponse = httpClient.GetAsync(this.Location).Result;
            var httpContent = httpResponse.Content.ReadAsStringAsync().Result;

            var schemaDocument = new XmlDocument();
            schemaDocument.LoadXml(httpContent);
            schemaDocument.DocumentElement.ChildNodes
                                          .GetNodes()
                                          .Where(n => n.Name.Contains("element"))
                                          .ToList()
                                          .ForEach(n =>
                                          {
                                              SchemaElements.Add(new SchemaElement(n));
                                          });

        }

        internal Schema(XmlNode node)
        {
            this.Namespace = node.Attributes.GetNamedItem("targetNamespace").Value;

            node.ChildNodes.GetNodes()
                           .Where(n => n.Name.Contains("element"))
                           .ToList()
                           .ForEach(n =>
                           {
                                SchemaElements.Add(new SchemaElement(n));
                           });
        }
    }
}
