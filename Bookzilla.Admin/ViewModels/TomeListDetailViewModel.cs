using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Net;
using System.Windows.Input;

namespace Bookzilla.Admin.ViewModels;

public class TomeListDetailViewModel : ObservableObject, INavigationAware
{
    private readonly IStoreTome _tomeService;
    private readonly DialogService _dialogService;
    private readonly IStorePublication _publicationService;
    private readonly ICoverExtractor _coverExtractor;
    private readonly INavigationService _navigationService;
    private ICommand _LoadImgCommand;
    private ICommand _OverrideImgCommand;
    private ICommand _SaveCommand;
    private ICommand _GoogleSearchCommand;
    private ICommand _ChangeImgCommand;
    //private ICommand _SaveCommand;
    public ICommand LoadImgCommand => _LoadImgCommand ?? (_LoadImgCommand = new RelayCommand(LoadImg));
    public ICommand OverrideImgCommand => _OverrideImgCommand ?? (_OverrideImgCommand = new RelayCommand(OverrideImg));
    public ICommand SaveCommand => _SaveCommand ?? (_SaveCommand = new RelayCommand<IObsToShow>(Save));
    public ICommand GoogleSearchCommand => _GoogleSearchCommand ?? (_GoogleSearchCommand = new RelayCommand(GoogleSearch));
    public ICommand ChangeImgCommand => _ChangeImgCommand ?? (_ChangeImgCommand = new RelayCommand(ChangeImg));


    private ObsTome _item;

    public ObsTome Item
    {
        get { return _item; }
        set { SetProperty(ref _item, value); }
    }
    private string _fanartTmpPath;
    public string FanartTmpPath
    {
        get { return _fanartTmpPath; }
        set { SetProperty(ref _fanartTmpPath, value); }
    }
    public TomeListDetailViewModel(IStorePublication publicationService, DialogService dialogService, IStoreTome tomeService, ICoverExtractor coverExtractor, INavigationService navigationService)
    {
        _publicationService = publicationService;
        _tomeService = tomeService;
        _dialogService = dialogService;
        _coverExtractor = coverExtractor;
        _navigationService = navigationService;
    }

    private void ChangeImg()
    {
        FanartTmpPath = _dialogService.ImgFilePicker();
    }
    public async void OnNavigatedTo(object parameter)
    {
        await InitValue(parameter);
    }
    private async Task InitValue(object parameter)
    {
        if (parameter is int ID)
        {
            Item = new ObsTome(await _tomeService.GetTomeByID(ID));
            FanartTmpPath = Item.Illustration;
        }
    }
    private void GoogleSearch()
    {
        //_navigationService.NavigateTo(typeof(TomeGoogleSynchroSearchViewModel).FullName, Item);
        var selectedbook = _dialogService.SearchBookInfo(Item);
        if(selectedbook != null)
        {
            var updatedItem = _dialogService.ReconcileBookInfo(Item, selectedbook);
            if(updatedItem != null)
            {
                Item = updatedItem;
                _dialogService.ShowInfo("Tome mis à jour");
            }
        }
    }
    public void OnNavigatedFrom()
    {
    }
    private async void OverrideImg()
    {
        var publication = await _publicationService.GetPublicationByID(Item.PublicationId);
        publication.CoverPath = Item.CoverPath;
        await _publicationService.PutPublication(publication);
    }
    private async void LoadImg()
    {
        using (var client = new WebClient())
        {
            var ext = Path.GetExtension(Item.FilePath);
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"));
            var tmpfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp", $"{Guid.NewGuid()}{ext}");
            var url = Path.Combine($"http://192.168.1.17:800{Item.FilePath}");
            client.DownloadFile(url, tmpfile);
            var tmpcover = _coverExtractor.GetCoverStream(tmpfile);
            if (tmpcover != null)
            {
                var result = await _tomeService.PostCoverTome(tmpcover, Item.Id, Item.PublicationId); 
            }
            //File.Delete(tmpfile);
            //File.Delete(tmpcover);
        }
        await InitValue(Item.Id);
        Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"), true);
    }
    private async void Save(IObsToShow obj)
    {
        if (FanartTmpPath != Item.Illustration)
        {
                var result = await _tomeService.PostCoverTome(FanartTmpPath, Item.Id, Item.PublicationId);
                var coverpath = Path.Combine("uploads", "Tome", Item.PublicationId.ToString(), "Cover", $"{Item.Id}.jpg");
                Item.CoverPath = coverpath;
        }
        var resultput = await _tomeService.PutTome(Item.Tome);
        _dialogService.ShowInfo(resultput);
    }
}
