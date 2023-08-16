using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Store
{
    public class StoreCollection : IStoreCollection
    {
        private IEnumerable<Collection> Collections;
        private readonly ICollectionAPIClient _collectionService;

        public StoreCollection(ICollectionAPIClient collectionService)
        {
            _collectionService = collectionService;
        }

        private async Task<int> LoadIfNull(bool force = false)
        {
            if (Collections == null || force == true)
                Collections = await _collectionService.GetCollections();
            return Collections.Count();
        }

        public IEnumerable<Collection> GetCollections()
        {
            foreach (var item in Collections)
                yield return item;
        }
        public async IAsyncEnumerable<Collection> GetCollectionsAsync()
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Collections)
                yield return item;
        }
        public async IAsyncEnumerable<Collection> GetCollectionsByParentID(int id)
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Collections.Where(x => x.ParentId == id))
                yield return item;
        }
        public async Task<Collection> GetCollectionByID(int id)
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            return Collections.FirstOrDefault(x => x.Id == id);
        }
        public async Task<string> PostCoverCollection(String filepath)
        {
            var task = _collectionService.PostCoverCollection(filepath).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PostCollection(CreateCollection item)
        {
            var task = _collectionService.PostCollection(item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PutCollection(Collection item)
        {
            var task = _collectionService.PutCollection(item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        //public async Task<Collection> GetOneRandomCollection()
        //{
        //    LoadIfNull();
        //    return await _collectionService.GetOneRandomCollection();
        //}
    }
}
