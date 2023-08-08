using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class TomeAPIClient : BaseApiClient, ITomeAPIClient
    {
        private string DeleteTomesURL;
        private string PutTomesURL = @"tome/update.php";
        private string PostTomeCoverURL = @"tome/uploadcover.php";
        private string PostTomeFileURL = @"tome/uploadfile.php";
        private string PostTomesURL = @"tome/create.php";
        private string GetTomesURL = @"tome/read.php";
        private string GetOneTomeURL = @"tome/single_read.php?Id={0}";
        private string GetTomeByParentURL = @"tome/readbyparent.php?Id={0}";
        private string GetOneRandomTomeURL = @"tome/singlerandomread.php";
        public TomeAPIClient() : base() { }
        public async Task<string> PostFileTome(String filepath, CreateTome item)
        {
            var request = new RestRequest(PostTomeFileURL, Method.Post);
            request.AlwaysMultipartFormData= true;
            request.AddFile("file", filepath);
            request.AddParameter("rank", item.livre.rank);
            request.AddParameter("publicationId", item.livre.publicationId);
            request.AddParameter("name", item.livre.Name);
            request.AddParameter("isEpub", item.livre.IsEpub);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<string> PostCoverTome(String filepath,int tomeId,int publicationId)
        {
            var request = new RestRequest(PostTomeCoverURL, Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddFile("file", filepath, "multipart/form-data");
            request.AddParameter("tomeId", tomeId);
            request.AddParameter("publicationId", publicationId);
            var response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<string> PutTome(Tome item)
        {
            var request = new RestRequest(Path.Combine(BaseApi, PutTomesURL), Method.Put);
            var jsondata = JsonConvert.SerializeObject(item);
            request.AddStringBody(jsondata, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<string> PostTome(CreateTome item)
        {
            var request = new RestRequest(Path.Combine(BaseApi, PostTomesURL), Method.Post);
            var jsondata = JsonConvert.SerializeObject(item);
            request.AddStringBody(jsondata, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<IEnumerable<Tome>> GetTomes()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetTomesURL), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            ListJSONTome result = JsonConvert.DeserializeObject<ListJSONTome>(responseText);
            return result.body;
        }
        public async Task<IEnumerable<Tome>> GetTomesByParentID(int id)
        {
            var request = new RestRequest(Path.Combine(BaseApi, string.Format(GetTomeByParentURL, id)), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            ListJSONTome result = JsonConvert.DeserializeObject<ListJSONTome>(responseText);
            return result.body;
        }
        public async Task<Tome> GetTomeByID(int id)
        {
            var request = new RestRequest(Path.Combine(BaseApi, string.Format(GetOneTomeURL, id)), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            Tome result = JsonConvert.DeserializeObject<Tome>(responseText);
            return result;
        }
        public async Task<Tome> GetOneRandomTome()
        {
            var request = new RestRequest(Path.Combine(BaseApi, GetOneRandomTomeURL), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = TextToJson(response.Content);
            Tome result = JsonConvert.DeserializeObject<Tome>(responseText);
            return result;
        }
    }
}
