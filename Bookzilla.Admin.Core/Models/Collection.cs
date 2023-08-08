using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FanartPath { get; set; } = @"uploads/Collection/default.jpg";
        public int ParentId { get; set; }
    }
    public class CreateCollection
    {
        public string Name { get; set; }
        public string FanartPath { get; set; } = @"uploads/Collection/default.jpg";
        public int ParentId { get; set; }
        public CreateCollection(Collection item) 
        { 
            Name = item.Name;
            FanartPath= item.FanartPath;
            ParentId = item.ParentId;
        }

        public CreateCollection()
        {
        }
    }
}
