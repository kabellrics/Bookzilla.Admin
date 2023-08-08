using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoverPath { get; set; } = @"uploads/Publication/default.png";
        public int CollectionId { get; set; }
        public string IsFavorite { get; set; }
    }
    public class CreatePublication
    {
        public string Name { get; set; }
        public string CoverPath { get; set; } = @"uploads/Publication/default.png";
        public int CollectionId { get; set; }
        public CreatePublication(Publication item)
        {
            Name = item.Name;
            CoverPath = item.CoverPath;
            CollectionId = item.CollectionId;
        }
        public CreatePublication()
        {
        }
    }
}
