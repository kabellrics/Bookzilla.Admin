using System.Collections.ObjectModel;
using System.Windows.Input;

using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookzilla.Admin.ViewModels;

public class PublicationListViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPublicationAPIClient _publicationService;
    private readonly ICollectionAPIClient _collectionService;
    private readonly DialogService _dialogService;
    private ICommand _navigateToDetailCommand;
    private ICommand _navigateToCreateCommand;

    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<ObsPublication>(NavigateToDetail));
    public ICommand NavigateToCreateCommand => _navigateToCreateCommand ?? (_navigateToCreateCommand = new RelayCommand(NavigateToCreate));

    public ObservableCollection<ObsPublication> Source { get; } = new ObservableCollection<ObsPublication>();

    public PublicationListViewModel(ICollectionAPIClient collectionService, IPublicationAPIClient publicationService, INavigationService navigationService, DialogService dialogService)
    {
        _collectionService = collectionService;
        _publicationService = publicationService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await InitiateList();
    }

    private async Task InitiateList()
    {
        Source.Clear();

        // Replace this with your actual data
        //var data = await _sampleDataService.GetContentGridDataAsync();
        var datapublication = await _publicationService.GetPublications();
        foreach (var item in datapublication)
        {
            Source.Add(new ObsPublication(item));
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void NavigateToDetail(ObsPublication publication)
    {
        _navigationService.NavigateTo(typeof(PublicationListDetailViewModel).FullName, publication.Id);
    }
    private async void NavigateToCreate()
    {
        var ParentPathList = new List<KeyValuePair<int, String>>();
        var datacollection = await _collectionService.GetCollections();
        foreach (var item in datacollection)
        {
            ParentPathList.Add(new KeyValuePair<int, String>(item.Id, GetFather(item, datacollection)));
        }

        var newObj = _dialogService.AddCollection(ParentPathList);
        var newitem = new CreatePublication() { CollectionId = newObj.CollectionId, Name = newObj.Name };
        var result = await _publicationService.PostPublication(newitem);
        _dialogService.ShowInfo(result);
        await InitiateList();
    }

    private string GetFather(Collection collection, IEnumerable<Collection> datacollection)
    {
        var father = datacollection.ToList().FirstOrDefault(x => x.Id == collection.ParentId);
        if (father != null)
        {
            return GetFather(father, datacollection) + "/" + collection.Name;
        }
        else return collection.Name;
    }
}
