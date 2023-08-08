using Bookzilla.Admin.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Admin.ViewModels.ObservableObj
{
    public class ObsCollection : ObservableObject, IObsToShow
    {
        public Collection Collection;
        public ObsCollection(Collection item)
        {
            this.Collection = item;
        }
        public ObsCollection()
        {
            this.Collection = new Collection();
            HasChanged = false;
        }
        private bool _hasChanged;
        public bool HasChanged { get => _hasChanged; set => SetProperty(ref _hasChanged, value); }
        public int Id
        {
            get => Collection.Id;
            set => SetProperty(Collection.Id, value, Collection, (syteme, item) => Collection.Id = item);
        }
        public string Name
        {
            get => Collection.Name;
            set
            {
                SetProperty(Collection.Name, value, Collection, (syteme, item) => Collection.Name = item);
            }
        }
        public string FanartPath
        {
            get => Collection.FanartPath;
            set
            {
                SetProperty(Collection.FanartPath, value, Collection, (syteme, item) => Collection.FanartPath = item);
            }
        }
        public string Illustration
        {
            get
            {
                if (string.IsNullOrEmpty(Collection.FanartPath))
                    return string.Empty;
                else
                    return @"http://192.168.1.17:800/" + Collection.FanartPath;
            }
        }
        public int CollectionId
        {
            get => Collection.ParentId;
            set => SetProperty(Collection.ParentId, value, Collection, (syteme, item) => Collection.ParentId = item);
        }
    }
}
