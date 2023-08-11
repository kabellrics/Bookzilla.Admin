using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class ParamAPIClient : BaseApiClient, IParamAPIClient
    {
        private string GetParamsURL = @"parametre/read.php";
        private IEnumerable<Param> _params;
        public ParamAPIClient() : base() { GetParamsAsync(); }

        public String GoogleBookAPIKey
        {
            get
            {
                if (_params is null)
                    GetParamsAsync();
                return _params.FirstOrDefault(x => x.Id == 1)?.Valeur;
            }
        }
        public String GoogleBookAPISearch
        {
            get
            {
                if (_params is null)
                    GetParamsAsync();
                return _params.FirstOrDefault(x => x.Id == 2)?.Valeur;
            }
        }
        public String GoogleBookAPIDetail
        {
            get
            {
                if (_params is null)
                    GetParamsAsync();
                return _params.FirstOrDefault(x => x.Id == 3)?.Valeur;
            }
        }
        public String GoogleBookBaseURL
        {
            get
            {
                if (_params is null)
                    GetParamsAsync();
                return _params.FirstOrDefault(x => x.Id == 4)?.Valeur;
            }
        }
        private async void GetParamsAsync()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetParamsURL), Method.Get);
            RestResponse response = client.Execute(request);
            var responseText = TextToJson(response.Content);
            ListJSONParam result = JsonConvert.DeserializeObject<ListJSONParam>(responseText);
            _params = result.body;
        }
    }
}
