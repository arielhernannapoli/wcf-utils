using System;
using System.Text.Json;
using System.Threading.Tasks;
using WcfCore.Helpers;

namespace WcfUtils
{
    class Program
    {
        //const string K_URI_SERVICE = "http://www.dneonline.com/calculator.asmx?wsdl";
        const string K_URI_SERVICE = "http://localhost:81/Service1.svc?wsdl";

        static async Task Main(string[] args)
        {
            var wsdl = await WsdlHelper.Build(K_URI_SERVICE);
            Console.WriteLine(JsonSerializer.Serialize(wsdl));
            Console.ReadLine();
        }
    }
}
