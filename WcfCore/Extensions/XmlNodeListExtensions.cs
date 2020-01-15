using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WcfCore.Extensions
{
    public static class XmlNodeListExtensions
    {
        public static List<XmlNode> GetNodes(this XmlNodeList xmlNodeList)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                nodes.Add(xmlNodeList.Item(i));
            }
            return nodes;
        }
    }
}
