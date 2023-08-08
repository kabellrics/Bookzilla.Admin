using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface IPublicationAPIClient
    {
        Task<Publication> GetPublicationByID(int id);
        Task<IEnumerable<Publication>> GetPublications();
        Task<IEnumerable<Publication>> GetPublicationsByParentID(int id);
        Task<Publication> GetOneRandomPublication();
        Task<string> PostPublication(CreatePublication item);
        Task<string> PutPublication(Publication item);
        Task<string> PostCoverPublication(String filepath);
    }
}