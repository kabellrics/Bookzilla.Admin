using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Store
{
    public class StoreTome : IStoreTome
    {
        private IEnumerable<Tome> Tomes;
        private readonly ITomeAPIClient _tomeService;
        public StoreTome(ITomeAPIClient tomeService)
        {
            _tomeService = tomeService;
        }
        private async Task<int> LoadIfNull(bool force = false)
        {
            if (Tomes == null || force == true)
                Tomes = await _tomeService.GetTomes();
            return Tomes.Count();
        }
        public async IAsyncEnumerable<Tome> GetTomesAsync()
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Tomes)
                yield return item;
        }
        public async IAsyncEnumerable<Tome> GetFavTomesAsync()
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Tomes.Where(x=>x.IsFavorite == "1").OrderBy(x => x.PublicationId).Take(10))
                yield return item;
        }
        public async IAsyncEnumerable<Tome> GetCurrentReadTomesAsync()
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Tomes.Where(x=>x.ReadingStatusId == 2).OrderBy(x => x.PublicationId).Take(10))
                yield return item;
        }
        public IEnumerable<Tome> GetTomes()
        {
            foreach (var item in Tomes)
                yield return item;
        }
        public async IAsyncEnumerable<Tome> GetTomesByParentID(int id)
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            foreach (var item in Tomes.Where(x => x.PublicationId == id))
                yield return item;
        }
        public async Task<Tome> GetTomeByID(int id)
        {
            var t = Task.Run(async () => await LoadIfNull());
            var result = await Task.WhenAll(t);
            return Tomes.FirstOrDefault(x => x.Id == id);
        }
        public async Task<string> PostFileTome(String filepath, CreateTome item)
        {
            var task = _tomeService.PostFileTome(filepath, item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PostCoverTome(String filepath, int tomeId, int publicationId)
        {
            var task = _tomeService.PostCoverTome(filepath, tomeId, publicationId).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PutTome(Tome item)
        {
            var task = _tomeService.PutTome(item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
        public async Task<string> PostTome(CreateTome item)
        {
            var task = _tomeService.PostTome(item).ContinueWith(x => { LoadIfNull(true); return x; });
            return await await task;
        }
    }
}
