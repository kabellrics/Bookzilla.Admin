using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Bookzilla.Admin.ViewModels;

public class MainViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IStoreTome _tomeService;
    private readonly IStorePublication _publicationService;
    private ICommand _navigateToPubliDetailCommand;
    private ICommand _navigateToTomeDetailCommand;
    public ICommand NavigateToDetailPubliCommand => _navigateToPubliDetailCommand ?? (_navigateToPubliDetailCommand = new RelayCommand<ObsPublication>(NavigateToDetailPubli));
    public ICommand NavigateToDetailTomeCommand => _navigateToTomeDetailCommand ?? (_navigateToTomeDetailCommand = new RelayCommand<ObsTome>(NavigateToDetailTome));

    public ObservableCollection<ObsPublication> FavPublication { get; } = new ObservableCollection<ObsPublication>();
    public ObservableCollection<ObsTome> FavTome { get; } = new ObservableCollection<ObsTome>();
    public ObservableCollection<ObsTome> ReadingTome { get; } = new ObservableCollection<ObsTome>();


    public MainViewModel(IStoreTome tomeService, IStorePublication publicationService, INavigationService navigationService)
    {
        _tomeService = tomeService;
        _publicationService = publicationService;
        _navigationService = navigationService;
    }

    public void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

    private void NavigateToDetailTome(ObsTome order)
    {
        _navigationService.NavigateTo(typeof(TomeListDetailViewModel).FullName, order.Id);
    }
    private void NavigateToDetailPubli(ObsPublication publication)
    {
        _navigationService.NavigateTo(typeof(PublicationListDetailViewModel).FullName, publication.Id);
    }
    public async void OnNavigatedTo(object parameter)
    {
        await InitiateList();
    }

    private async Task InitiateList()
    {
        await InitFavPublication();
        await InitFavTome();
        await InitReadingTome();
    }

    private async Task InitReadingTome()
    {
        ReadingTome.Clear();
        await foreach (var item in _tomeService.GetCurrentReadTomesAsync())
        {
            ReadingTome.Add(new ObsTome(item));
        }
    }

    private async Task InitFavTome()
    {
        FavTome.Clear();
        await foreach (var item in _tomeService.GetFavTomesAsync())
        {
            FavTome.Add(new ObsTome(item));
        }
    }

    private async Task InitFavPublication()
    {
        FavPublication.Clear();
        await foreach (var item in _publicationService.GetFavPublicationsAsync())
        {
            FavPublication.Add(new ObsPublication(item));
        }
    }

}
