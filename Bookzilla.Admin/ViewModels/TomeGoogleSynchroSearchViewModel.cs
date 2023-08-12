using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models.GoogleBook;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookzilla.Admin.ViewModels
{
    public class TomeGoogleSynchroSearchViewModel : ObservableObject, INavigationAware
    {
        private readonly IGoogleBookAPIClient _GoogleBookService;
        private readonly INavigationService _navigationService;
        private ICommand _GoogleReconcileCommand;
        private ICommand _GoBackCommand;
        public ICommand GoogleReconcileCommand => _GoogleReconcileCommand ?? (_GoogleReconcileCommand = new RelayCommand(GoogleReconcile));
        public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand(GoBack));


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
        public TomeGoogleSynchroSearchViewModel(IGoogleBookAPIClient googleBookService, INavigationService navigationService)
        {
            _GoogleBookService = googleBookService;
            _navigationService = navigationService;
        }

        public void OnNavigatedFrom()
        {
        }

        private void GoogleReconcile()
        {
            _navigationService.NavigateTo(typeof(TomeGooglereconcileViewModel).FullName, new object[]{ Selectedbook,Item});
        }
        private void GoBack()
        {
            _navigationService.GoBack();
        }
        public async void OnNavigatedTo(object parameter)
        {
            await InitValue(parameter);
        }

        private async Task InitValue(object parameter)
        {
            if (parameter is ObsTome tome)
            {
                Item = tome;
                Source.Clear();
                await foreach(var googlebook in _GoogleBookService.SearchForGoogleBookbyNameAsync(Item.Name))
                {
                    Source.Add(googlebook);
                }
            }
        }
    }
}
