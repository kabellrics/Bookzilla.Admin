using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Store
{
    public class StoreCollection
    {
        private IEnumerable<Collection> Collections;
        private readonly ICollectionAPIClient _collectionService;

        public StoreCollection(ICollectionAPIClient collectionService)
        {
            _collectionService = collectionService;
        }

        private void LoadIfNull(bool force = false)
        {
            if(Collections == null || force == true)
                Collections = _collectionService.GetCollections().Result;
        }

        public async IAsyncEnumerable<Collection> GetAllCollections()
        {
            LoadIfNull();
            foreach (var item in Collections)
                yield return item;
        }
    }
}
