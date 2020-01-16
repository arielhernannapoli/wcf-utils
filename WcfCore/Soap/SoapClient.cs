using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace WcfCore.Soap
{
	public class SoapClient : ISoapClient
	{
		private readonly HttpClient _client;
		private readonly string _serviceUrl;
		private readonly string _serviceNamespace;

		public SoapClient(string serviceUrl, string serviceNamespace)
		{
			_client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
			_serviceUrl = serviceUrl;
			_serviceNamespace = serviceNamespace;
		}

		private void SetSoapAction(string method)
		{
			const string header = "SOAPAction";
			if (_client.DefaultRequestHeaders.Contains(header))
			{
				_client.DefaultRequestHeaders.Remove(header);
			}
			_client.DefaultRequestHeaders.Add(header, $"{_serviceNamespace}");
		}

		private SoapError GetError(string xml)
		{
			XDocument xmlDoc = XDocument.Parse(xml);
			XNamespace xmlns = "http://schemas.xmlsoap.org/soap/envelope/";
			var fault = xmlDoc.Descendants(xmlns + "Fault").FirstOrDefault();
			if (fault != null)
			{
				return new SoapError
				{
					Code = fault.Element(xmlns + "faultcode")?.Value ??
						   fault.Element("faultcode")?.Value,
					Message =
						fault.Element(xmlns + "faultstring")?.Value ??
						fault.Element("faultstring")?.Value,
					Detail =
						fault.Element(xmlns + "detail")?.Value ??
						fault.Element("detail")?.Value
				};
			}

			xmlns = "http://www.w3.org/2003/05/soap-envelope";
			fault = xmlDoc.Descendants(xmlns + "Fault").FirstOrDefault();
			if (fault != null)
			{
				return new SoapError
				{
					Code = fault.Element(xmlns + "Code")?.Value,
					Message = fault.Element(xmlns + "Reason")?.Value,
					Detail = fault.Element(xmlns + "Detail")?.Value
				};
			}

			return null;
		}

        public async Task<string> PostAsync(string method, XmlDocument document)
		{
			SetSoapAction(method);

			var response = await _client.PostAsync(_serviceUrl, new StringContent(document.OuterXml, Encoding.UTF8, "text/xml"));

			string xmlResult = await response.Content.ReadAsStringAsync();

			SoapError error = GetError(xmlResult);

			if (error != null)
			{
				throw new HttpRequestException(error.Message);
			}

			return xmlResult;
		}

	}
}
