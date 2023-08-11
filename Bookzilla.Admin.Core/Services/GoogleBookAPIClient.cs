using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.Core.Models.GoogleBook;
using Bookzilla.Admin.Core.Models.GoogleBook.Raw;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class GoogleBookAPIClient : IGoogleBookAPIClient
    {
        private readonly IParamAPIClient _paramService;
        public GoogleBookAPIClient(IParamAPIClient paramService)
        {
            _paramService = paramService;
            Init();
        }

        private async void Init()
        {
            var options = new RestClientOptions(_paramService.GoogleBookBaseURL);
            client = new RestClient(options);
        }
        private RestClient client { get; set; }

        public async IAsyncEnumerable<GoogleBook> SearchForGoogleBookbyNameAsync(string name)
        {
            var fullrequestpath = string.Format(_paramService.GoogleBookAPISearch, name, _paramService.GoogleBookAPIKey);
            var request = new RestRequest(Path.Combine(_paramService.GoogleBookBaseURL, fullrequestpath), Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var responseText = response.Content;
            var idList = ExtractIdListFromJson(JsonConvert.DeserializeObject<GoogleBookRawData>(responseText));
            foreach (var id in idList)
            {
                var detailrequestpath = string.Format(_paramService.GoogleBookAPIDetail, id, _paramService.GoogleBookAPIKey);
                var detailrequest = new RestRequest(Path.Combine(_paramService.GoogleBookBaseURL, detailrequestpath), Method.Get);
                RestResponse detailresponse = await client.ExecuteAsync(detailrequest);
                var detailresponseText = detailresponse.Content;
                GoogleBookDetail result = JsonConvert.DeserializeObject<GoogleBookDetail>(detailresponseText);
                yield return ExtractRawGoogleBookData(result);
            }
        }

        private IEnumerable<string> ExtractIdListFromJson(GoogleBookRawData Content)
        {
            List<string> idList = new List<string>();

            try
            {
                return Content.items.Select(x => x.id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }

            return idList;
        }

        public GoogleBook ExtractRawGoogleBookData(GoogleBookDetail rawdata)
        {
            GoogleBook googleBook = new GoogleBook();
            try
            {
                googleBook.GoogleId = rawdata.id;
                googleBook.Title = rawdata.volumeInfo.title;
                if(rawdata.volumeInfo.authors != null)
                googleBook.Authors = string.Join(",", rawdata.volumeInfo?.authors);
                googleBook.PublishedDate = rawdata.volumeInfo?.publishedDate;
                if (rawdata.volumeInfo?.description != null)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(rawdata.volumeInfo?.description);
                    googleBook.Description = doc.DocumentNode.InnerText; 
                }
                //googleBook.Description = rawdata.volumeInfo?.description;
                if (rawdata.volumeInfo.industryIdentifiers != null)
                {
                    googleBook.ISBN_10 = rawdata.volumeInfo?.industryIdentifiers.FirstOrDefault(x => x.type == "ISBN_10").identifier;
                    googleBook.ISBN_13 = rawdata.volumeInfo?.industryIdentifiers.FirstOrDefault(x => x.type == "ISBN_13").identifier;
                }
                if (rawdata.volumeInfo.imageLinks != null)
                {
                    if (!string.IsNullOrEmpty(rawdata.volumeInfo?.imageLinks?.extraLarge))
                        googleBook.ImagePath = rawdata.volumeInfo?.imageLinks?.extraLarge;
                    else if (!string.IsNullOrEmpty(rawdata.volumeInfo?.imageLinks?.large))
                        googleBook.ImagePath = rawdata.volumeInfo?.imageLinks?.large;
                    else if (!string.IsNullOrEmpty(rawdata.volumeInfo?.imageLinks?.medium))
                        googleBook.ImagePath = rawdata.volumeInfo?.imageLinks?.medium;
                    else if (!string.IsNullOrEmpty(rawdata.volumeInfo?.imageLinks?.small))
                        googleBook.ImagePath = rawdata.volumeInfo?.imageLinks?.small;
                    else if (!string.IsNullOrEmpty(rawdata.volumeInfo?.imageLinks?.thumbnail))
                        googleBook.ImagePath = rawdata.volumeInfo?.imageLinks?.thumbnail;
                    else if (!string.IsNullOrEmpty(rawdata.volumeInfo?.imageLinks?.smallThumbnail))
                        googleBook.ImagePath = rawdata.volumeInfo?.imageLinks?.smallThumbnail;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return googleBook;
        }
    }
}
