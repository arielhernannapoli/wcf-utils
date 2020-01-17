using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WcfCore.Helpers;
using WcfCore.Soap;
using WcfWeb.Helpers;
using WcfWeb.Models;

namespace WcfWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new IndexModel());
        }

        public async Task<List<String>> GetMethodsWS(string uri)
        {
            var wsdl = await WsdlHelper.Build($"{uri}?wsdl");
            return wsdl.Definitions.PortType.Operations.Select(o => o.Name).ToList();
        }

        public async Task<String> GetMethodRequest(string uri, string method)
        {
            var soapRequest = await SoapHelper.BuildSoapRequest(uri, method);
            return XmlFormatParserHelper.GetFormattedXml(soapRequest);
        }

        public async Task<String> InvokeMethodWS(string uri, string method, string request)
        {
            var wsdl = await WsdlHelper.Build($"{uri}?wsdl");
            var soapClient = new SoapClient(uri, wsdl.Definitions.Binding.OperationsBinding.First(o => o.Name == method).Action);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(request);
            var soapResponse = await soapClient.PostAsync("POST", xmlDocument);
            return XmlFormatParserHelper.GetFormattedXml(soapResponse);
        }
    }
}
