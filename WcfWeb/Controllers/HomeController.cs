using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WcfCore.Helpers;
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

        public async Task<IActionResult> ConsultarWS(IndexModel indexModel)
        {
            var wsdl = await WsdlHelper.Build($"{indexModel.ServiceUri}?wsdl");

            indexModel.ServiceMethods.Clear();
            wsdl.Definitions.PortType.Operations.ForEach(o =>
            {
                indexModel.ServiceMethods.Add(o.Name);
            });

            if (!String.IsNullOrEmpty(indexModel.ServiceMethod))
            {
                var soapRequest = await SoapHelper.BuildSoapRequest(indexModel.ServiceUri, indexModel.ServiceMethod);
                indexModel.SoapRequest = XmlFormatParserHelper.GetFormattedXml(soapRequest);
            }

            return View("Index", indexModel);
        }
    }
}
