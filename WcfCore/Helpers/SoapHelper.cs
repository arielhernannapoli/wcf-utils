using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WcfCore.Models;

namespace WcfCore.Helpers
{
    public static class SoapHelper
    {
        public async static Task<XmlDocument> BuildSoapRequest(string uriService, string operation)
        {
            var soapRequest = new XmlDocument();
            
            var soapEnvelope = soapRequest.CreateElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            var soapHeader = soapRequest.CreateElement("soap", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
            var soapBody = soapRequest.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");

            var wsdl = await WsdlHelper.Build(uriService+"?wsdl");
            var operationData = wsdl.Definitions.PortType.Operations.FirstOrDefault(o => o.Name == operation);
            var messageData = wsdl.Definitions.Messages.FirstOrDefault(m => RemoveNs(operationData.Input.Message) == m.Name);
            SchemaElement parametersData = null;
            wsdl.Definitions.Types[0].Schemas.ForEach(s =>
            {
                if(parametersData == null)
                    parametersData = s.SchemaElements.Find(e => RemoveNs(messageData.Parameter) == e.Name);
            });

            var soapOperation = soapRequest.CreateElement(operationData.Name, wsdl.Definitions.Attributes.First(a => a.Name == messageData.Parameter.Split(':')[0]).Value);
            parametersData.SchemaElements.ForEach(e =>
            {
                var soapParameter = soapRequest.CreateElement(e.Name, wsdl.Definitions.Attributes.First(a => a.Name == messageData.Parameter.Split(':')[0]).Value);
                soapParameter.InnerText = "5";
                soapOperation.AppendChild(soapParameter);
            });
            soapBody.AppendChild(soapOperation);

            soapEnvelope.AppendChild(soapHeader);
            soapEnvelope.AppendChild(soapBody);

            soapRequest.AppendChild(soapEnvelope);
            return soapRequest;
        }

        public static dynamic Invoke(string url, string soapEnvelope)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string strResult = string.Empty;
            byte[] data = encoding.GetBytes(soapEnvelope);
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "POST";
            webrequest.ContentType = "text/xml";
            webrequest.ContentLength = data.Length;
            Stream newStream = webrequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
            StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            strResult = loResponseStream.ReadToEnd();
            loResponseStream.Close();
            webresponse.Close();
            strResult = strResult.Replace("</string>", "");
            return strResult;
        }

        private static string RemoveNs(string nodeValue)
        {
            return nodeValue.Contains(":") ? nodeValue.Split(':')[1] : nodeValue;

        }
    }
}
