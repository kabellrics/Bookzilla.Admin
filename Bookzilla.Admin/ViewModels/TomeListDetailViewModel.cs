﻿using Bookzilla.Admin.Contracts.ViewModels;
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
    private readonly ITomeAPIClient _tomeService;
    private readonly DialogService _dialogService;
    private readonly IPublicationAPIClient _publicationService;
    private readonly ICoverExtractor _coverExtractor;
    private ICommand _LoadImgCommand;
    private ICommand _OverrideImgCommand;
    private ICommand _SaveCommand;
    //private ICommand _SaveCommand;
    public ICommand LoadImgCommand => _LoadImgCommand ?? (_LoadImgCommand = new RelayCommand(LoadImg));
    public ICommand OverrideImgCommand => _OverrideImgCommand ?? (_OverrideImgCommand = new RelayCommand(OverrideImg));
    public ICommand SaveCommand => _SaveCommand ?? (_SaveCommand = new RelayCommand<IObsToShow>(Save));

    private ObsTome _item;

    public ObsTome Item
    {
        get { return _item; }
        set { SetProperty(ref _item, value); }
    }
    public TomeListDetailViewModel(IPublicationAPIClient publicationService, DialogService dialogService, ITomeAPIClient tomeService, ICoverExtractor coverExtractor)
    {
        _publicationService = publicationService;
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
            Item = new ObsTome(await _tomeService.GetTomeByID(ID));
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
            var tmpfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp", $"tmpfile{ext}");
            var url = Path.Combine($"http://192.168.1.17:800{Item.FilePath}");
            client.DownloadFile(url, tmpfile);
            var tmpcover = _coverExtractor.GetCoverStream(tmpfile);
            var result = await _tomeService.PostCoverTome(tmpcover, Item.Id, Item.PublicationId);
            //File.Delete(tmpfile);
            //File.Delete(tmpcover);
        }
        await InitValue(Item.Id);
    }
    private async void Save(IObsToShow obj)
    {
        var resultput = await _tomeService.PutTome(Item.Tome);
        _dialogService.ShowInfo(resultput);
    }
}