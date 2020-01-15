using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Definitions
    {
        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();
        public List<Types> Types { get; private set; } = new List<Types>();
        public List<Message> Messages { get; private set; } = new List<Message>();
        public PortType PortType { get; private set; }
        public Binding Binding { get; private set; }
        public Service Service { get; private set; }

        internal Definitions(XmlElement element)
        {
            element.Attributes.GetNodes().ForEach(n =>
            {
                Attributes.Add(new Attribute(n.Prefix, n.LocalName, n.NamespaceURI, n.Value));
            });

            element.ChildNodes.GetNodes()
                                .Where(n => n.Name.Contains("types"))
                                .ToList()
                                .ForEach(n =>
                                {
                                    Types.Add(new Types(n));
                                });

            element.ChildNodes.GetNodes()
                                .Where(n => n.Name.Contains("message"))
                                .ToList()
                                .ForEach(n =>
                                {
                                    Messages.Add(new Message(n));
                                });

            PortType = new PortType(element.ChildNodes
                                           .GetNodes()
                                           .FirstOrDefault(n => n.Name.Contains("portType")));

            Binding = new Binding(element.ChildNodes
                                         .GetNodes()
                                         .FirstOrDefault(n => n.Name.Contains("binding")));

            Service = new Service(element.ChildNodes
                             .GetNodes()
                             .FirstOrDefault(n => n.Name.Contains("service")));
        }
    }
}
