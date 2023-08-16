using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface IStoreTome
    {
        Task<Tome> GetTomeByID(int id);
        IEnumerable<Tome> GetTomes();
        IAsyncEnumerable<Tome> GetTomesAsync();
        IAsyncEnumerable<Tome> GetFavTomesAsync();
        IAsyncEnumerable<Tome> GetCurrentReadTomesAsync();
        IAsyncEnumerable<Tome> GetTomesByParentID(int id);
        Task<string> PostCoverTome(string filepath, int tomeId, int publicationId);
        Task<string> PostFileTome(string filepath, CreateTome item);
        Task<string> PostTome(CreateTome item);
        Task<string> PutTome(Tome item);
    }
}