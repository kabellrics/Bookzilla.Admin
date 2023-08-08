using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface ITomeAPIClient
    {
        Task<Tome> GetOneRandomTome();
        Task<Tome> GetTomeByID(int id);
        Task<IEnumerable<Tome>> GetTomes();
        Task<IEnumerable<Tome>> GetTomesByParentID(int id);
        Task<string> PostCoverTome(String filepath, int tomeId, int publicationId);
        Task<string> PostFileTome(string filepath, CreateTome item);
        Task<string> PostTome(CreateTome item);
        Task<string> PutTome(Tome item);
    }
}