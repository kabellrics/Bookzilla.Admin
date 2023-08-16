using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Store
{
    public class StorePublication
    {
        private IEnumerable<Publication> Publications;
        private readonly IPublicationAPIClient _publicationService;

        public StorePublication(IPublicationAPIClient publicationService)
        {
            _publicationService = publicationService;
        }
        private void LoadIfNull(bool force = false)
        {
            if (Publications == null || force == true)
                Publications = _publicationService.GetPublications().Result;
        }
    }
}
