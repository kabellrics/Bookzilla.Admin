using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Store
{
    public class StoreTome
    {
        private IEnumerable<Tome> Tomes;
        private readonly ITomeAPIClient _tomeService;
        public StoreTome(ITomeAPIClient tomeService)
        {
            _tomeService = tomeService;
        }
        private void LoadIfNull(bool force = false)
        {
            if (Tomes == null || force == true)
                Tomes = _tomeService.GetTomes().Result;
        }
    }
}
