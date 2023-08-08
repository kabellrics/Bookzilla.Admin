using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Admin.Dialogs.AddCollectionDialog
{
    public class AddCollectionViewModel: DialogViewModelBase
    {
        private ObsCollection _item;
        public ObsCollection Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private int _parentId;
        public int ParentId
        {
            get { return _parentId; }
            set { SetProperty(ref _parentId, value); }
        }
        private string _fanartTmpPath;
        public string FanartTmpPath
        {
            get { return _fanartTmpPath; }
            set { SetProperty(ref _fanartTmpPath, value); }
        }
        private readonly ICollectionAPIClient _collectionService;
        public ObservableCollection<KeyValuePair<int, String>> Parent { get; } = new ObservableCollection<KeyValuePair<int, String>>();

        public AddCollectionViewModel(IEnumerable<KeyValuePair<int, String>> Source)
        {
            Item = new ObsCollection();
            Parent.Clear();
            Parent.Add(new KeyValuePair<int, string>(0, "Aucune"));
            foreach (var item in Source)
            {
                Parent.Add(item);
            }
        }
    }
}
