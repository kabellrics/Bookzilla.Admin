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
using Microsoft.VisualBasic;

namespace Bookzilla.Admin.ViewModels;

public class CollectionListViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IStoreCollection _collectionService;
    private readonly DialogService _dialogService;
    private ICommand _navigateToDetailCommand;
    private ICommand _navigateToCreateCommand;
    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<ObsCollection>(NavigateToDetail));
    public ICommand NavigateToCreateCommand => _navigateToCreateCommand ?? (_navigateToCreateCommand = new RelayCommand(NavigateToCreate));

    public ObservableCollection<ObsCollection> Source { get; } = new ObservableCollection<ObsCollection>();

    public CollectionListViewModel(IStoreCollection collectionService, INavigationService navigationService, DialogService dialogService)
    {
        _collectionService = collectionService;
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
        await foreach(var item in _collectionService.GetCollectionsAsync())
        {
            Source.Add(new ObsCollection(item));
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void NavigateToDetail(ObsCollection collection)
    {
        _navigationService.NavigateTo(typeof(CollectionListDetailViewModel).FullName, collection.Id);
    }
    private async void NavigateToCreate()
    {
        var ParentPathList = new List<KeyValuePair<int, String>>();
        foreach(var item in Source)
        {
            ParentPathList.Add(new KeyValuePair<int, String>(item.Id, GetFather(item)));
        }

        var newObj = _dialogService.AddCollection(ParentPathList);
        var newitem = new CreateCollection() { ParentId = newObj.Id, Name = newObj.Name };
        var result = await _collectionService.PostCollection(newitem);
        _dialogService.ShowInfo( result);
        await InitiateList();
    }

    private string GetFather(ObsCollection collection)
    {
        var father = Source.ToList().FirstOrDefault(x => x.Id == collection.CollectionId);
        if (father != null)
        {
            return GetFather(father) + "/"+ collection.Name;
        }
        else return collection.Name;
    }
}
