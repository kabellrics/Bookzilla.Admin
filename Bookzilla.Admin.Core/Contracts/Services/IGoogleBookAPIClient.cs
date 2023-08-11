using Bookzilla.Admin.Core.Models.GoogleBook;

namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface IGoogleBookAPIClient
    {
        IAsyncEnumerable<GoogleBook> SearchForGoogleBookbyNameAsync(string name);
    }
}