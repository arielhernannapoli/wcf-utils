using System.Xml;

namespace WcfCore.Models
{
    public class Wsdl
    {
        public Definitions Definitions { get; private set; }

        public Wsdl(string content)
        {
            var contract = new XmlDocument();
            contract.LoadXml(content);
            var definitions = contract.DocumentElement;
            this.Definitions = new Definitions(definitions);
        }
    }
}
