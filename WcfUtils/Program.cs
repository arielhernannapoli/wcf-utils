using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WcfCore.Helpers;
using WcfCore.Soap;

namespace WcfUtils
{
    class Program
    {
        //const string K_URI_SERVICE = "http://www.dneonline.com/calculator.asmx";
        //const string K_OPERATION = "Add";

        const string K_URI_SERVICE = "http://localhost:8090/Service1.svc";
        const string K_OPERATION = "GetDataUsingDataContract";

        static async Task Main(string[] args)
        {
            var wsdl = await WsdlHelper.Build(K_URI_SERVICE + "?wsdl");
            var soapRequest = await SoapHelper.BuildSoapRequest(K_URI_SERVICE, K_OPERATION);
            var soapClient = new SoapClient(K_URI_SERVICE, wsdl.Definitions.Binding.OperationsBinding.First(o=>o.Name == K_OPERATION).Action);
            var soapResponse = await soapClient.PostAsync("POST", soapRequest);
        }
    }
}
