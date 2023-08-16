using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface IStorePublication
    {
        Task<Publication> GetPublicationByID(int id);
        IEnumerable<Publication> GetPublications();
        IAsyncEnumerable<Publication> GetPublicationsAsync();
        IAsyncEnumerable<Publication> GetFavPublicationsAsync();
        IAsyncEnumerable<Publication> GetPublicationsByParentID(int id);
        Task<string> PostCoverPublication(string filepath);
        Task<string> PostPublication(CreatePublication item);
        Task<string> PutPublication(Publication item);
    }
}