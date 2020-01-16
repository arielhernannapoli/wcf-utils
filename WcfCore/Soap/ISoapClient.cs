using System.Threading.Tasks;
using System.Xml;

namespace WcfCore.Soap
{
    public interface ISoapClient
    {
        Task<string> PostAsync(string method, XmlDocument document);
    }
}