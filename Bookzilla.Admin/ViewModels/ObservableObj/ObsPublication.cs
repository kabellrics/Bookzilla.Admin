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
    public class ObsPublication : ObservableObject, IObsToShow
    {
        public Publication Publication;
        public ObsPublication(Publication item) 
        {
            this.Publication = item;
        }
        public ObsPublication()
        {
            this.Publication = new Publication();
            HasChanged = false;
        }
        private bool _hasChanged;
        public bool HasChanged { get => _hasChanged; set => SetProperty(ref _hasChanged, value); }
        public int Id
        {
            get => Publication.Id;
            set => SetProperty(Publication.Id, value, Publication, (syteme, item) => Publication.Id = item);
        }
        public string Name
        {
            get => Publication.Name;
            set
            {
                SetProperty(Publication.Name, value, Publication, (syteme, item) => Publication.Name = item);
            }
        }
        public string CoverPath
        {
            get => Publication.CoverPath;
            set
            {
                SetProperty(Publication.CoverPath, value, Publication, (syteme, item) => Publication.CoverPath = item);
            }
        }
        public string Illustration
        {
            get {
                if (string.IsNullOrEmpty(Publication.CoverPath))
                    return string.Empty;
                else
                    return @"http://192.168.1.17:800/" + Publication.CoverPath;
            }
        }
        public int CollectionId
        {
            get => Publication.CollectionId;
            set => SetProperty(Publication.CollectionId, value, Publication, (syteme, item) => Publication.CollectionId = item);
        }
        public bool IsFavorite
        {
            get => Publication.IsFavorite == "1";
            set {
                var valuetosave = value ? "1" : "0";
                SetProperty(Publication.IsFavorite, valuetosave, Publication, (syteme, item) => Publication.IsFavorite = item);
            }
        }
    }
}
