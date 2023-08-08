namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface ICoverExtractor
    {
        string GetCoverStream(string path);
    }
}