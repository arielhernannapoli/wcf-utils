using System;
using System.Net.Http;
using System.Threading.Tasks;
using WcfCore.Models;

namespace WcfCore.Helpers
{
    public static class WsdlHelper
    {
        public async static Task<Wsdl> Build(string uri)
        {
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            return new Wsdl(httpContent);
        }
    }
}
