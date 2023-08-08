using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class BaseApiClient
    {
        public String BaseApi = @"http://192.168.1.17:800/api";
        public RestClient client { get; set; }
        public BaseApiClient() {
            var options = new RestClientOptions(BaseApi);
            client = new RestClient(options);
        }

        protected string TextToJson(string text)
        {
            var startIndex = text.IndexOf('{');
            if (startIndex >= 0)
            {
                return text.Substring(startIndex); 
            }
            else return string.Empty;
        }
    }
}
