namespace Bookzilla.Admin.ViewModels.ObservableObj
{
    public interface IObsToShow
    {
        string Illustration { get; }
        int Id { get; set; }
        string Name { get; set; }
        bool HasChanged { get; set; }
        int CollectionId { get; set; }
    }
    public enum SpecificListParameter
    {
        All = 0,
        Favorite = 1,
        UnRead = 2,
        Reading = 3,
        Read = 4
    }
}