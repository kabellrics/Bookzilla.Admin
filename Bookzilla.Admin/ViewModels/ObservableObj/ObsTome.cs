using Bookzilla.Admin.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Admin.ViewModels.ObservableObj
{
    public class ObsTome : ObservableObject, IObsToShow
    {
        public Tome Tome;
        public ObsTome(Tome item)
        {
            this.Tome = item;
        }
        public ObsTome()
        {
            this.Tome = new Tome();
            HasChanged = false;
        }
        private bool _hasChanged;
        public bool HasChanged { get => _hasChanged; set => SetProperty(ref _hasChanged, value); }
        public bool CanOverridePubliCover { get { return !CoverPath.Contains("default"); } }

        public int Id
        {
            get => Tome.Id;
            set => SetProperty(Tome.Id, value, Tome, (syteme, item) => Tome.Id = item);
        }
        public string Name
        {
            get => Tome.Name;
            set
            {
                SetProperty(Tome.Name, value, Tome, (syteme, item) => Tome.Name = item);
            }
        }
        public string CoverPath
        {
            get => Tome.CoverPath;
            set
            {
                SetProperty(Tome.CoverPath, value, Tome, (syteme, item) => Tome.CoverPath = item);
            }
        }
        public string FilePath
        {
            get => Tome.FilePath;
            set
            {
                SetProperty(Tome.FilePath, value, Tome, (syteme, item) => Tome.FilePath = item);
            }
        }
        public string Illustration
        {
            get
            {
                if (string.IsNullOrEmpty(Tome.CoverPath))
                    return string.Empty;
                else
                    return @"http://192.168.1.17:800/" + Tome.CoverPath;
            }
        }
        public int PublicationId
        {
            get => Tome.PublicationId;
            set => SetProperty(Tome.PublicationId, value, Tome, (syteme, item) => Tome.PublicationId = item);
        }
        public int OrderInPublication
        {
            get => Tome.OrderInPublication;
            set => SetProperty(Tome.OrderInPublication, value, Tome, (syteme, item) => Tome.OrderInPublication = item);
        }
        public int CurrentPage
        {
            get => Tome.CurrentPage;
            set => SetProperty(Tome.CurrentPage, value, Tome, (syteme, item) => Tome.CurrentPage = item);
        }
        public int ReadingStatusId
        {
            get => Tome.ReadingStatusId;
            set => SetProperty(Tome.ReadingStatusId, value, Tome, (syteme, item) => Tome.ReadingStatusId = item);
        }
        public bool IsFavorite
        {
            get => Tome.IsFavorite == "1";
            set
            {
                var valuetosave = value ? "1":"0"; 
                SetProperty(Tome.IsFavorite, valuetosave, Tome, (syteme, item) => Tome.IsFavorite = item);
            }
        }
        public bool IsEpub
        {
            get => Tome.IsEpub == "1";
            set
            {
                var valuetosave = value ? "1":"0"; 
                SetProperty(Tome.IsEpub, valuetosave, Tome, (syteme, item) => Tome.IsEpub = item);
            }
        }

        public string GoogleBookId
        {
            get => Tome.GoogleBookId;
            set
            {
                SetProperty(Tome.GoogleBookId, value, Tome, (syteme, item) => Tome.GoogleBookId = item);
            }
        }
        public string Auteur
        {
            get => Tome.Auteur;
            set
            {
                SetProperty(Tome.Auteur, value, Tome, (syteme, item) => Tome.Auteur = item);
            }
        }
        public string Description
        {
            get => Tome.Description;
            set
            {
                SetProperty(Tome.Description, value, Tome, (syteme, item) => Tome.Description = item);
            }
        }
        public string PublicationDate
        {
            get => Tome.PublicationDate;
            set
            {
                SetProperty(Tome.PublicationDate, value, Tome, (syteme, item) => Tome.PublicationDate = item);
            }
        }
        public string ReadingStatus
        {
            get => ((ReadingStatus)Tome.ReadingStatusId).ToString("D");
        }
        public string Size
        {
            get => GetReadableSize(Tome.Size);
        }
        public int CollectionId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private string GetReadableSize(int size)
        {
            if (size < 1024)
            {
                return $"{size} octets";
            }
            else if (size < 1024 * 1024)
            {
                double fileSizeInKb = size / 1024.0;
                return $"{fileSizeInKb} Ko";
            }
            else
            {
                double fileSizeInMb = size / (1024.0 * 1024.0);
                return $"{fileSizeInMb} Mo";
            }
        }
    }
}
