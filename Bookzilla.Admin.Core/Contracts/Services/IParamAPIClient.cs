namespace Bookzilla.Admin.Core.Contracts.Services
{
    public interface IParamAPIClient
    {
        string GoogleBookAPIDetail { get; }
        string GoogleBookAPIKey { get; }
        string GoogleBookAPISearch { get; }
        string GoogleBookBaseURL { get; }
    }
}