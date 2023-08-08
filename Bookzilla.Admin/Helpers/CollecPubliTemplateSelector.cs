using Bookzilla.Admin.ViewModels.ObservableObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookzilla.Admin.Helpers
{
    public class CollecPubliTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CollectionTemplate { get; set; }

        public DataTemplate PublicationTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedTemplate = this.CollectionTemplate;
            if (item is ObsPublication)
                selectedTemplate = this.PublicationTemplate;
            return selectedTemplate;
        }
    }
}
