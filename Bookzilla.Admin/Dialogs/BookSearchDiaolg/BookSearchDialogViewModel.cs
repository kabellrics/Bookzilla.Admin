using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models.GoogleBook;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookzilla.Admin.Dialogs.BookSearchDiaolg
{
    public class BookSearchDialogViewModel : DialogViewModelBase
    {
        private readonly IGoogleBookAPIClient _GoogleBookService;
        private ICommand _GoogleReconcileCommand;
        private ICommand _GoBackCommand;
        public ICommand GoogleReconcileCommand => _GoogleReconcileCommand ?? (_GoogleReconcileCommand = new RelayCommand<object>(GoogleReconcile));
        public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand<object>(GoBack));


        public ObservableCollection<GoogleBook> Source { get; } = new ObservableCollection<GoogleBook>();
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
        public BookSearchDialogViewModel(ObsTome tome)
        {
            Item = tome;
            _GoogleBookService = App.Current.GetService<IGoogleBookAPIClient>();// Ioc.Default.GetService<IGoogleBookAPIClient>();
            Source.Clear();
            InitResult();
        }

        private async void InitResult()
        {
            await foreach (var googlebook in _GoogleBookService.SearchForGoogleBookbyNameAsync(Item.Name))
            {
                Source.Add(googlebook);
            }
        }

        private void GoogleReconcile(object parameter)
        {
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
    }
}
