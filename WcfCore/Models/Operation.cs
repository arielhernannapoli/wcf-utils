using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WcfCore.Extensions;

namespace WcfCore.Models
{
    public class Operation
    {
        public string Name { get; private set; }
        public InputOutput Input { get; private set; }
        public InputOutput Output { get; private set; }

        internal Operation(XmlNode node)
        {
            this.Name = node.Attributes.GetNamedItem("name").Value;

            this.Input = new InputOutput(node.ChildNodes
                                            .GetNodes()
                                            .FirstOrDefault(n => n.Name.Contains("input"))
                                            .Attributes
                                            .GetNodes()
                                            .FirstOrDefault(n => n.Name.Contains("Action"))
                                            .Value,
                                         node.ChildNodes
                                            .GetNodes()
                                            .FirstOrDefault(n => n.Name.Contains("input"))
                                            .Attributes
                                            .GetNamedItem("message")
                                            .Value);

            this.Output = new InputOutput(node.ChildNodes
                                .GetNodes()
                                .FirstOrDefault(n => n.Name.Contains("output"))
                                .Attributes
                                .GetNodes()
                                .FirstOrDefault(n => n.Name.Contains("Action"))
                                .Value,
                             node.ChildNodes
                                .GetNodes()
                                .FirstOrDefault(n => n.Name.Contains("output"))
                                .Attributes
                                .GetNamedItem("message")
                                .Value);

        }
    }
}
