using Bookzilla.Admin.Contracts.ViewModels;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Models.GoogleBook;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Admin.ViewModels
{
    public class TomeGoogleSynchroSearchViewModel : ObservableObject, INavigationAware
    {
        private readonly IGoogleBookAPIClient _GoogleBookService;
        public ObservableCollection<GoogleBook> Source { get; } = new ObservableCollection<GoogleBook>();
        private ObsTome _item;

        public ObsTome Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
        public TomeGoogleSynchroSearchViewModel(IGoogleBookAPIClient googleBookService)
        {
            _GoogleBookService = googleBookService;
        }

        public void OnNavigatedFrom()
        {
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
