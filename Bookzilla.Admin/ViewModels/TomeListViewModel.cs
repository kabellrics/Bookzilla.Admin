using System.Collections.ObjectModel;
using System.Windows.Input;

using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookzilla.Admin.ViewModels;

public class TomeListViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly ITomeAPIClient _tomeService;
    private ICommand _navigateToDetailCommand;

    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<ObsTome>(NavigateToDetail));

    public ObservableCollection<ObsTome> Source { get; } = new ObservableCollection<ObsTome>();

    public TomeListViewModel(ITomeAPIClient tomeService, INavigationService navigationService)
    {
        _tomeService = tomeService;
        _navigationService = navigationService;
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
        var datapublication = await _tomeService.GetTomes();
        await foreach (var item in GetTomes(datapublication.OrderBy(x => x.PublicationId).ThenBy(x => x.OrderInPublication)))
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
    private async IAsyncEnumerable<ObsTome> GetTomes(IEnumerable<Tome> tomes)
    {
        foreach (var tome in tomes)
            yield return new ObsTome(tome);
    }
    private void NavigateToDetail(ObsTome order)
    {
        _navigationService.NavigateTo(typeof(TomeListDetailViewModel).FullName, order.Id);
    }
}
