using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WcfCore.Extensions
{
    public static class XmlNamedNodeMapExtensions
    {
        public static List<XmlNode> GetNodes(this XmlNamedNodeMap xmlNamedNodeMap)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            for (int i = 0; i < xmlNamedNodeMap.Count; i++)
            {
                nodes.Add(xmlNamedNodeMap.Item(i));
            }
            return nodes;
        }
    }
}
