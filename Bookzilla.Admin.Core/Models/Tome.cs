using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Models
{
    public class Tome
    {
        public int Id;
        public string Name;
        public string CoverPath;
        public int PublicationId;
        public int CurrentPage;
        public string FilePath;
        public int OrderInPublication;
        public int ReadingStatusId;
        public int Size;
        public string IsFavorite;
        public string IsEpub;
        public string CFI_EPUB;
        public string GoogleBookId;
        public string Auteur;
        public string Description;
        public string PublicationDate;
        public string ISBN_10;
        public string ISBN_13;
    }
    public class CreateTome
    {
        public Livre livre { get; set; }
        public CreateTome(Tome tome)
        {
            livre = new Livre() { Name = tome.Name, publicationId = tome.PublicationId.ToString(), rank = tome.OrderInPublication.ToString(), IsEpub = tome.IsEpub };
        }
        public class Livre
        {
            public string rank { get; set; }
            public string publicationId { get; set; }
            public string Name { get; set; }
            public string IsEpub { get; set; }
        }
    }
}
