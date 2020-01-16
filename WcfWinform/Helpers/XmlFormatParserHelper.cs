using System.IO;
using System.Text;
using System.Xml;

namespace WcfWinform.Helpers
{
    public static class XmlFormatParserHelper
    {
        public static string GetFormattedXml(XmlDocument xmlDocument)
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    xmlDocument.WriteTo(xmlTextWriter);
                }
            }
            return builder.ToString();
        }

        public static string GetFormattedXml(string content)
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(content);
                    xmlDocument.WriteTo(xmlTextWriter);
                }
            }
            return builder.ToString();
        }

    }
}
