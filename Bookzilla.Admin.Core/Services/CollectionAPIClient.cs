using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class CollectionAPIClient : BaseApiClient, ICollectionAPIClient
    {
        private string DeleteCollectionsURL;
        private string PutCollectionsURL = @"collection/update.php";
        private string PostCollectionCoverURL = @"collection/uploadfanart.php";
        private string PostCollectionsURL = @"collection/create.php";
        private string GetCollectionsURL = @"collection/read.php";
        private string GetOneCollectionURL = @"collection/single_read.php?Id={0}";
        private string GetCollectionByParentURL = @"collection/readbyparent.php?Id={0}";
        private string GetOneRandomCollectionURL = @"collection/singlerandomread.php";
        public CollectionAPIClient() : base() { }

        public async Task<string> PostCoverCollection(String filepath)
        {
            var request = new RestRequest(PostCollectionCoverURL)
            .AddFile("file", filepath, "multipart/form-data");
            var response = await client.PostAsync(request);
            return response.Content;
        }
        public async Task<string> PutCollection(Collection item)
        {
            var request = new RestRequest(Path.Combine(BaseApi, PutCollectionsURL), Method.Put);
            var jsondata = JsonConvert.SerializeObject(item);
            request.AddStringBody(jsondata, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<string> PostCollection(CreateCollection item)
        {
            var request = new RestRequest(Path.Combine(BaseApi, PostCollectionsURL), Method.Post);
            var jsondata = JsonConvert.SerializeObject(item);
            request.AddStringBody(jsondata, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<IEnumerable<Collection>> GetCollections()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetCollectionsURL), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            ListJSONCollection result = JsonConvert.DeserializeObject<ListJSONCollection>(responseText);
            return result.body;
        }
        public async Task<IEnumerable<Collection>> GetCollectionsByParentID(int id)
        {
            var request = new RestRequest(Path.Combine(BaseApi, string.Format(GetCollectionByParentURL, id)), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            ListJSONCollection result = JsonConvert.DeserializeObject<ListJSONCollection>(responseText);
            return result.body;
        }
        public async Task<Collection> GetCollectionByID(int id)
        {
            var request = new RestRequest(Path.Combine(BaseApi, string.Format(GetOneCollectionURL, id)), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            Collection result = JsonConvert.DeserializeObject<Collection>(responseText);
            return result;
        }
        public async Task<Collection> GetOneRandomCollection()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetOneRandomCollectionURL), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            Collection result = JsonConvert.DeserializeObject<Collection>(responseText);
            return result;
        }
    }
}
