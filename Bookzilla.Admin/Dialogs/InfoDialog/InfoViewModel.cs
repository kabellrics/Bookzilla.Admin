using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.ViewModels.ObservableObj;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Admin.Dialogs.InfoDialog
{
    public class InfoViewModel : DialogViewModelBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        public InfoViewModel(String Source)
        {
            Text = FormatJson(Source);
        }
        private string FormatJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            string formattedJson;
            try
            {
                var parsedJson = JToken.Parse(json);
                formattedJson = parsedJson.ToString(Formatting.Indented);
            }
            catch
            {
                formattedJson = json;
            }

            return formattedJson;
        }
    }
}
