using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlzEx.Standard;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Windows.Input;
using Windows.Web.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Bookzilla.Admin.ViewModels;

public class PublicationListDetailViewModel : ObservableObject, INavigationAware
{
    private readonly IPublicationAPIClient _publicationService;
    private readonly ICollectionAPIClient _collectionService;
    private readonly INavigationService _navigationService;
    private readonly ICoverExtractor _coverExtractor;
    private readonly ITomeAPIClient _tomeService;
    private readonly DialogService _dialogService;
    private ICommand _ChangeImgCommand;
    private ICommand _SaveCommand;
    private ICommand _AddFilesCommand;
    private ICommand _ExtractCoversCommand;
    private ICommand _navigateToDetailCommand;
    public ICommand ChangeImgCommand => _ChangeImgCommand ?? (_ChangeImgCommand = new RelayCommand<IObsToShow>(ChangeImg));
    public ICommand SaveCommand => _SaveCommand ?? (_SaveCommand = new RelayCommand<IObsToShow>(Save));
    public ICommand AddFilesCommand => _AddFilesCommand ?? (_AddFilesCommand = new RelayCommand(AddFiles));
    public ICommand ExtractCoversCommand => _ExtractCoversCommand ?? (_ExtractCoversCommand = new RelayCommand(ExtractCovers));
    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<ObsTome>(NavigateToDetail));


    private ObsPublication _item;

    public ObsPublication Item
    {
        get { return _item; }
        set { SetProperty(ref _item, value); }
    }
    private String _hierarchie;
    public String Hierarchie
    {
        get { return _hierarchie; }
        set { SetProperty(ref _hierarchie, value); }
    }
    private string _fanartTmpPath;
    public string FanartTmpPath
    {
        get { return _fanartTmpPath; }
        set { SetProperty(ref _fanartTmpPath, value); }
    }
    public ObservableCollection<ObsTome> Childs { get; } = new ObservableCollection<ObsTome>();

    public PublicationListDetailViewModel(ICollectionAPIClient collectionService, INavigationService navigationService, IPublicationAPIClient publicationService, DialogService dialogService, ITomeAPIClient tomeService, ICoverExtractor coverExtractor)
    {
        _collectionService = collectionService;
        _publicationService = publicationService;
        _navigationService = navigationService;
        _tomeService = tomeService;
        _dialogService = dialogService;
        _coverExtractor = coverExtractor;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await InitValue(parameter);
    }

    private async Task InitValue(object parameter)
    {
        if (parameter is int ID)
        {
            Childs.Clear();
            Item = new ObsPublication(await _publicationService.GetPublicationByID(ID));
            FanartTmpPath = Item.Illustration;
            var fathercollection = await _collectionService.GetCollectionByID(Item.CollectionId);
            var dataallcollection = await _collectionService.GetCollections();
            Hierarchie = GetFather(fathercollection, dataallcollection);
            var childs = await _tomeService.GetTomesByParentID(ID);
            if (childs != null)
            {
                foreach (var item in childs)
                {
                    Childs.Add(new ObsTome(item));
                } 
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
    private String GetFather(Collection item, IEnumerable<Collection> dataallcollection)
    {
        var father = dataallcollection.ToList().FirstOrDefault(x => x.Id == item.ParentId);
        if (father != null)
        {
            return GetFather(father, dataallcollection) + " / " + item.Name;
        }
        else return item.Name;
    }
    private void ChangeImg(IObsToShow obj)
    {
        FanartTmpPath = _dialogService.ImgFilePicker();
    }
    private async void Save(IObsToShow obj)
    {
        if (Item.Illustration != FanartTmpPath)
        {
            var filename = Item.Id + Path.GetExtension(FanartTmpPath);
            var coverpath = @"uploads/Publication/Cover/" + filename;
            Item.CoverPath = coverpath;
            var result = await _publicationService.PutPublication(Item.Publication);
            _dialogService.ShowInfo(result);
            var FolderTmp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "Publication");
            Directory.CreateDirectory(FolderTmp);
            File.Copy(FanartTmpPath, Path.Combine(FolderTmp, filename));
            result = await _publicationService.PostCoverPublication(Path.Combine(FolderTmp, filename));
            _dialogService.ShowInfo(result);
            File.Delete(Path.Combine(FolderTmp, filename));
        }
        var resultput = await _publicationService.PutPublication(Item.Publication);
        _dialogService.ShowInfo(resultput);
    }

    private int GetStartingRank()
    {
        if (Childs.Count == 0) return 1;
        else return Childs.Max(x => x.OrderInPublication) + 1;
    }
    private async void AddFiles()
    {
        var files = _dialogService.FileFilePicker();
        var ind = GetStartingRank();
        foreach ( var file in files)
        {
            var isEpub = Path.GetExtension(file) == ".epub" ? "1":"0"; 
            var tome = new CreateTome(new Tome() { OrderInPublication = ind, PublicationId = Item.Id, Name = Path.GetFileNameWithoutExtension(file),IsEpub = isEpub });
            var t = Task.Run(async ()=> await SendFile(file, tome));
            var result = await Task.WhenAll(t);
            if(!result[0].Contains("successfully"))
                _dialogService.ShowInfo(result[0]);
            ind++;
        }
        _dialogService.ShowInfo("Fin du traitement");
        await InitValue(Item.Id);
    }
    private async void ExtractCovers()
    {
        foreach(var item in Childs)
        {
            var t = Task.Run(async () => await LoadCover(item));
            var result = await Task.WhenAll(t);//.ContinueWith(x=> 
            //{
            //    Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"), true);
            //    return x;
            //});
        }
        _dialogService.ShowInfo("Fin du traitement");
        await InitValue(Item.Id);
        Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"), true);
    }

    private async Task<string> LoadCover(ObsTome item)
    {
        using (var client = new System.Net.Http.HttpClient())
        {
            var ext = Path.GetExtension(item.FilePath);
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"));
            var tmpfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla","temp", $"{Guid.NewGuid()}{ext}");
            var url = Path.Combine($"http://192.168.1.17:800{item.FilePath}");
            //client.(url, tmpfile);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            await using var ms = await response.Content.ReadAsStreamAsync();
            await using var fs = File.Create(tmpfile);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
            fs.Close(); ms.Close();
            var tmpcover = _coverExtractor.GetCoverStream(tmpfile);
            if (tmpcover != null)
            {
                return await _tomeService.PostCoverTome(tmpcover, item.Id, item.PublicationId); 
            }
            return null;
        }
    }

    private async Task<string> SendFile(string file, CreateTome tome)
    {
        return await _tomeService.PostFileTome(file, tome);
    }
    private void NavigateToDetail(ObsTome item)
    {
        if (item is ObsTome)
            _navigationService.NavigateTo(typeof(TomeListDetailViewModel).FullName, item.Id);
    }
}
