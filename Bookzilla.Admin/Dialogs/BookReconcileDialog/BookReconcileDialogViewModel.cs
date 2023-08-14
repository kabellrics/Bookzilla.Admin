using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models.GoogleBook;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using Bookzilla.Admin.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Bookzilla.Admin.Dialogs.BookReconcileDialog
{
    public class BookReconcileDialogViewModel : DialogViewModelBase
    {
        private readonly ITomeAPIClient _tomeService;
        private readonly DialogService.DialogService _dialogService;
        private ICommand _ReconcileNameCommand;
        private ICommand _ReconcileGoogleIDCommand;
        private ICommand _ReconcileAuthorsCommand;
        private ICommand _ReconcilePublishedDateCommand;
        private ICommand _ReconcileDescriptionCommand;
        private ICommand _ReconcileISBN_10Command;
        private ICommand _ReconcileISBN_13Command;
        private ICommand _ReconcileCoverCommand;
        private ICommand _GoogleReconcileCommand;
        private ICommand _GoBackCommand;
        public ICommand GoogleReconcileCommand => _GoogleReconcileCommand ?? (_GoogleReconcileCommand = new RelayCommand<object>(GoogleReconcile));
        public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand<object>(GoBack));

        public BookReconcileDialogViewModel(GoogleBook book, ObsTome tome)
        {
            _tomeService = App.Current.GetService<ITomeAPIClient>();//Ioc.Default.GetService<ITomeAPIClient>();
            _dialogService = App.Current.GetService<DialogService.DialogService>();//Ioc.Default.GetService<DialogService.DialogService>();
            Selectedbook = book;
            Item = tome;
            FanartTmpPath = Item.Illustration;
        }
        private async void GoogleReconcile(object parameter)
        {
            if (FanartTmpPath != Item.Illustration)
            {
                using (var client = new WebClient())
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"));
                    var tmpfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp", $"{Guid.NewGuid()}.jpg");
                    client.DownloadFile(Selectedbook.ImagePath, tmpfile);
                    var result = await _tomeService.PostCoverTome(tmpfile, Item.Id, Item.PublicationId);
                    var coverpath = Path.Combine("uploads", "Tome", Item.PublicationId.ToString(), "Cover", $"{Item.Id}.jpg");
                    Item.CoverPath = coverpath;
                }
                Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp"), true);
            }
            var resultput = await _tomeService.PutTome(Item.Tome);
            CloseDialogWithResult(parameter as Window, true);
        }
        private void GoBack(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
        public void CloseDialogWithResult(Window dialog, bool result)
        {
            if (dialog != null)
                dialog.DialogResult = result;
        }
        public ICommand ReconcileNameCommand => _ReconcileNameCommand ?? (_ReconcileNameCommand = new RelayCommand(ReconcileName));
        private void ReconcileName()
        {
            Item.Name = Selectedbook.Title;
        }
        public ICommand ReconcileGoogleIDCommand => _ReconcileGoogleIDCommand ?? (_ReconcileGoogleIDCommand = new RelayCommand(ReconcileGoogleID));
        private void ReconcileGoogleID()
        {
            Item.GoogleBookId = Selectedbook.GoogleId;
        }
        public ICommand ReconcileAuthorsCommand => _ReconcileAuthorsCommand ?? (_ReconcileAuthorsCommand = new RelayCommand(ReconcileAuthors));
        private void ReconcileAuthors()
        {
            Item.Auteur = Selectedbook.Authors;
        }
        public ICommand ReconcilePublishedDateCommand => _ReconcilePublishedDateCommand ?? (_ReconcilePublishedDateCommand = new RelayCommand(ReconcilePublishedDate));
        private void ReconcilePublishedDate()
        {
            Item.PublicationDate = Selectedbook.PublishedDate;
        }
        public ICommand ReconcileDescriptionCommand => _ReconcileDescriptionCommand ?? (_ReconcileDescriptionCommand = new RelayCommand(ReconcileDescription));
        private void ReconcileDescription()
        {
            Item.Description = Selectedbook.Description;
        }
        public ICommand ReconcileISBN_10Command => _ReconcileISBN_10Command ?? (_ReconcileISBN_10Command = new RelayCommand(ReconcileISBN_10));
        private void ReconcileISBN_10()
        {
            Item.ISBN_10 = Selectedbook.ISBN_10;
        }
        public ICommand ReconcileISBN_13Command => _ReconcileISBN_13Command ?? (_ReconcileISBN_13Command = new RelayCommand(ReconcileISBN_13));
        private void ReconcileISBN_13()
        {
            Item.ISBN_13 = Selectedbook.ISBN_13;
        }
        public ICommand ReconcileCoverCommand => _ReconcileCoverCommand ?? (_ReconcileCoverCommand = new RelayCommand(ReconcileCover));
        private void ReconcileCover()
        {
            FanartTmpPath = Selectedbook.ImagePath;
        }

        private ObsTome _item;

        public ObsTome Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
        private GoogleBook _selectedbook;

        public GoogleBook Selectedbook
        {
            get { return _selectedbook; }
            set { SetProperty(ref _selectedbook, value); }
        }
        private string _fanartTmpPath;
        public string FanartTmpPath
        {
            get { return _fanartTmpPath; }
            set { SetProperty(ref _fanartTmpPath, value); }
        }
    }
}
