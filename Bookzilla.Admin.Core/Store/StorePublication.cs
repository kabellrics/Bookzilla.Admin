using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Store
{
    public class StorePublication : IStorePublication
    {
        private IEnumerable<Publication> Publications;
        private readonly IPublicationAPIClient _publicationService;

        public StorePublication(IPublicationAPIClient publicationService)
        {
            _publicationService = publicationService;
        }
        private async Task<int> LoadIfNull(bool force = false)
        {
            if (Publications == null || force == true)
                Publications = await _publicationService.GetPublications();
            return Publications.Count();
        }
        public async IAsyncEnumerable<Publication> GetPublicationsAsync()
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Publications)
                yield return item;
        }
        public async IAsyncEnumerable<Publication> GetFavPublicationsAsync()
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Publications.Where(x=>x.IsFavorite == "1"))
                yield return item;
        }
        public IEnumerable<Publication> GetPublications()
        {
            foreach (var item in Publications)
                yield return item;
        }
        public async IAsyncEnumerable<Publication> GetPublicationsByParentID(int id)
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Publications.Where(x => x.CollectionId == id))
                yield return item;
        }
        public async Task<Publication> GetPublicationByID(int id)
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            return Publications.FirstOrDefault(x => x.Id == id);
        }
        public async Task<string> PostCoverPublication(String filepath)
        {
            var task = _publicationService.PostCoverPublication(filepath).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PutPublication(Publication item)
        {
            var task = _publicationService.PutPublication(item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PostPublication(CreatePublication item)
        {
            var task = _publicationService.PostPublication(item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
    }
}
