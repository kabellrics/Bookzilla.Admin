using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class PublicationAPIClient : BaseApiClient, IPublicationAPIClient
    {
        private string DeletePublicationsURL;
        private string PutPublicationsURL = @"publication/update.php";
        private string PostPublicationCoverURL = @"publication/uploadcover.php";
        private string PostPublicationsURL = @"publication/create.php";
        private string GetPublicationsURL = @"publication/read.php";
        private string GetOnePublicationURL = @"publication/single_read.php?Id={0}";
        private string GetPublicationByParentURL = @"publication/readbyparent.php?Id={0}";
        private string GetOneRandomPublicationURL = @"publication/singlerandomread.php";
        public PublicationAPIClient() : base() { }
        public async Task<string> PostCoverPublication(String filepath)
        {
            var request = new RestRequest(PostPublicationCoverURL)
            .AddFile("file", filepath, "multipart/form-data");
            var response = await client.PostAsync(request);
            return response.Content;
        }
        public async Task<string> PutPublication(Publication item)
        {
            var request = new RestRequest(Path.Combine(BaseApi, PutPublicationsURL), Method.Put);
            var jsondata = JsonConvert.SerializeObject(item);
            request.AddStringBody(jsondata, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<string> PostPublication(CreatePublication item)
        {
            var request = new RestRequest(Path.Combine(BaseApi, PostPublicationsURL), Method.Post);
            var jsondata = JsonConvert.SerializeObject(item);
            request.AddStringBody(jsondata, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<IEnumerable<Publication>> GetPublications()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetPublicationsURL), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            ListJSONPublication result = JsonConvert.DeserializeObject<ListJSONPublication>(responseText);
            return result.body;
        }
        public async Task<IEnumerable<Publication>> GetPublicationsByParentID(int id)
        {
            var request = new RestRequest(Path.Combine(BaseApi, string.Format(GetPublicationByParentURL, id)), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            ListJSONPublication result = JsonConvert.DeserializeObject<ListJSONPublication>(responseText);
            return result.body;
        }
        public async Task<Publication> GetPublicationByID(int id)
        {
            var request = new RestRequest(Path.Combine(BaseApi, string.Format(GetOnePublicationURL, id)), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            Publication result = JsonConvert.DeserializeObject<Publication>(responseText);
            return result;
        }
        public async Task<Publication> GetOneRandomPublication()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetOneRandomPublicationURL), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            Publication result = JsonConvert.DeserializeObject<Publication>(responseText);
            return result;
        }
    }
}
