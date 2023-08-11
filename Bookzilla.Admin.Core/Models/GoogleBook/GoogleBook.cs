using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Models.GoogleBook
{
    public class GoogleBook
    {
        public bool IsSelected { get; set; }
        public string GoogleId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public string ISBN_10 { get; set; }
        public string ISBN_13 { get; set; }
        public string ImagePath { get; set; }
    }
}
