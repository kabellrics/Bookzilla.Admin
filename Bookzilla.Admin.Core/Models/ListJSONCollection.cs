using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Models
{
    public class ListJSONCollection
    {
        public List<Collection> body { get; set; }
        public int itemCount { get; set; }
    }
    public class ListJSONPublication
    {
        public List<Publication> body { get; set; }
        public int itemCount { get; set; }
    }
    public class ListJSONTome
    {
        public List<Tome> body { get; set; }
        public int itemCount { get; set; }
    }
    public class ListJSONParam
    {
        public List<Param> body { get; set; }
        public int itemCount { get; set; }
    }
}
