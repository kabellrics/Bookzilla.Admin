using Bookzilla.Admin.Core.Models.GoogleBook;
using Bookzilla.Admin.Dialogs.AddCollectionDialog;
using Bookzilla.Admin.Dialogs.BookReconcileDialog;
using Bookzilla.Admin.Dialogs.BookSearchDiaolg;
using Bookzilla.Admin.Dialogs.InfoDialog;
using Bookzilla.Admin.ViewModels.ObservableObj;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Admin.Dialogs.DialogService
{
    public class DialogService
    {
        public DialogResult OpenDialog()
        {
            DialogWindow win = new DialogWindow();
            win.ShowDialog();
            return DialogResult.Undefined;
        }
        public string[] FileFilePicker()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Books files (*.cbz;*.cbr;*.epub)|*.cbz;*.cbr;*.epub";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileNames;
            else return new string[0];
        }
        public string ImgFilePicker()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else return string.Empty;
        }
        public IObsToShow AddCollection(IEnumerable<KeyValuePair<int, String>> Source) 
        {
            DialogWindow dialog = new DialogWindow();
            var vm = new AddCollectionViewModel(Source);
            dialog.DataContext = vm;
            if (dialog.ShowDialog().Value)
            {
                return vm.Item;
            }
            return null;
        }
        public GoogleBook SearchBookInfo(ObsTome tome)
        {
            DialogWindow dialog = new DialogWindow();
            var vm = new BookSearchDialogViewModel(tome);
            dialog.DataContext = vm;
            if(dialog.ShowDialog().Value)
            {
                return vm.Selectedbook;
            }
            return null;
        }
        public ObsTome ReconcileBookInfo(ObsTome tome, GoogleBook book)
        {
            DialogWindow dialog = new DialogWindow();
            var vm = new BookReconcileDialogViewModel(book, tome);
            dialog.DataContext = vm;
            if (dialog.ShowDialog().Value)
            {
                return vm.Item;
            }
            return null;
        }
        public void ShowInfo(String Source) 
        {
            DialogWindow dialog = new DialogWindow();
            var vm = new InfoViewModel(Source);
            dialog.DataContext = vm;
            dialog.ShowDialog();
        }
    }
}
