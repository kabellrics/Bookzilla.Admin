using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Bookzilla.Admin.ViewModels;

public class CollectionListDetailViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IStoreCollection _collectionService;
    private readonly IStorePublication _publicationService;
    private readonly DialogService _dialogService;
    private ICommand _navigateToDetailCommand;
    private ICommand _ChangeImgCommand;
    private ICommand _SaveCommand;

    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<IObsToShow>(NavigateToDetail));
    public ICommand ChangeImgCommand => _ChangeImgCommand ?? (_ChangeImgCommand = new RelayCommand<IObsToShow>(ChangeImg));
    public ICommand SaveCommand => _SaveCommand ?? (_SaveCommand = new RelayCommand<IObsToShow>(Save));

    private ObsCollection _item;
    public ObsCollection Item
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
    public ObservableCollection<ObsCollection> ChildsCollections { get; } = new ObservableCollection<ObsCollection>();
    public ObservableCollection<ObsPublication> ChildsPublications { get; } = new ObservableCollection<ObsPublication>();
    public ObservableCollection<IObsToShow> Childs { get; } = new ObservableCollection<IObsToShow>();
    public CollectionListDetailViewModel(IStoreCollection collectionService, IStorePublication publicationService, INavigationService navigationService, DialogService dialogService)
    {
        _collectionService = collectionService;
        _publicationService = publicationService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is int ID)
        {
            Item = new ObsCollection(await _collectionService.GetCollectionByID(ID));
            FanartTmpPath = Item.Illustration;
            await foreach(var item in _collectionService.GetCollectionsByParentID(ID))
            {
                var obsitem = new ObsCollection(item);
                ChildsCollections.Add(obsitem);
                Childs.Add(obsitem);
            }
            #region old
            //var datacollection = await _collectionService.GetCollectionsByParentID(ID);
            //if (datacollection != null)
            //{
            //    foreach (var item in datacollection)
            //    {
            //        var obsitem = new ObsCollection(item);
            //        ChildsCollections.Add(obsitem);
            //        Childs.Add(obsitem);
            //    } 
            //} 
            #endregion
            await foreach(var item in _publicationService.GetPublicationsByParentID(ID))
            {
                var obsitem = new ObsPublication(item);
                ChildsPublications.Add(obsitem);
                Childs.Add(obsitem);
            }
            #region old
            //var datapublication = await _publicationService.GetPublicationsByParentID(ID);
            //if (datapublication != null)
            //{
            //    foreach (var item in datapublication)
            //    {
            //        var obsitem = new ObsPublication(item);
            //        ChildsPublications.Add(obsitem);
            //        Childs.Add(obsitem);
            //    }
            //} 
            #endregion
            var dataallcollection = _collectionService.GetCollections();
            Hierarchie = GetFather(Item.Collection, dataallcollection);
        }
    }
    private void NavigateToDetail(IObsToShow item)
    {
        if(item is ObsCollection)
        _navigationService.NavigateTo(typeof(CollectionListDetailViewModel).FullName, item.Id);
        else if (item is ObsPublication)
            _navigationService.NavigateTo(typeof(PublicationListDetailViewModel).FullName, item.Id);
    }
    public void OnNavigatedFrom()
    {
    }
    private string GetFather(Collection collection, IEnumerable<Collection> dataallcollection)
    {
        var father = dataallcollection.ToList().FirstOrDefault(x => x.Id == collection.ParentId);
        if (father != null)
        {
            return GetFather(father, dataallcollection) + " / " + collection.Name;
        }
        else return collection.Name;
    }
    private void ChangeImg(IObsToShow obj)
    {
        FanartTmpPath = _dialogService.ImgFilePicker();
    }
    private async void Save(IObsToShow obj)
    {
        var filename = Item.Id + Path.GetExtension(FanartTmpPath);
        var coverpath = @"uploads/Collection/Fanart/" + filename;
        Item.FanartPath = coverpath;
        var result = await _collectionService.PutCollection(Item.Collection);
        _dialogService.ShowInfo(result);
        var FolderTmp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "Collection");
        Directory.CreateDirectory(FolderTmp);
        File.Copy(FanartTmpPath, Path.Combine(FolderTmp, filename));
        result = await _collectionService.PostCoverCollection(Path.Combine(FolderTmp, filename));
        _dialogService.ShowInfo( result);
        File.Delete(Path.Combine(FolderTmp, filename));
    }
}
