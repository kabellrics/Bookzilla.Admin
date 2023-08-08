using Bookzilla.Admin.Core.Models;

namespace Bookzilla.Admin.Core.Contracts.Services;

public interface ISampleDataService
{
    Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();
}
