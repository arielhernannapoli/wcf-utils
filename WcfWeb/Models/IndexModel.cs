using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfWeb.Models
{
    public class IndexModel
    {
        public string ServiceUri { get; set; }
        public string ServiceMethod { get; set; }
        public List<string> ServiceMethods { get; set; } = new List<string>();
        public string SoapRequest { get; set; } = string.Empty;
        public string SoapResponse { get; set; } = string.Empty;
    }
}
