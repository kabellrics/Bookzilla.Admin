using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface IStoreCollection
    {
        Task<Collection> GetCollectionByID(int id);
        IEnumerable<Collection> GetCollections();
        IAsyncEnumerable<Collection> GetCollectionsAsync();
        IAsyncEnumerable<Collection> GetCollectionsByParentID(int id);
        Task<string> PostCollection(CreateCollection item);
        Task<string> PostCoverCollection(string filepath);
        Task<string> PutCollection(Collection item);
    }
}