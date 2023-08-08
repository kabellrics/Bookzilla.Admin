using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface ICollectionAPIClient
    {
        Task<Collection> GetCollectionByID(int id);
        Task<IEnumerable<Collection>> GetCollections();
        Task<IEnumerable<Collection>> GetCollectionsByParentID(int id);
        Task<Collection> GetOneRandomCollection();
        Task<string> PostCollection(CreateCollection item);
        Task<string> PutCollection(Collection item);
        Task<string> PostCoverCollection(String filepath);
    }
}