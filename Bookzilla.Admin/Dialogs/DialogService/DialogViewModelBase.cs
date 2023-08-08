using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Bookzilla.Admin.Dialogs.DialogService
{
    public class DialogViewModelBase : ObservableObject
    {
        protected string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        protected string _contentButtonYes;
        public string ContentButtonYes
        {
            get { return _contentButtonYes; }
            set { SetProperty(ref _contentButtonYes, value); }
        }

        protected string _contentButtonNo;
        public string ContentButtonNo
        {
            get { return _contentButtonNo; }
            set { SetProperty(ref _contentButtonNo, value); }
        }

        protected string _contentButtonCancel;
        public string ContentButtonCancel
        {
            get { return _contentButtonCancel; }
            set { SetProperty(ref _contentButtonCancel, value); }
        }

        protected ICommand _yesCommand = null;
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand<object>(OnYesClicked));
            }
        }

        protected ICommand _NoCommand = null;
        public ICommand NoCommand
        {
            get
            {
                return _NoCommand ?? (_NoCommand = new RelayCommand<object>(OnNoClicked));
            }
        }

        protected ICommand _CancelCommand = null;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand<object>(OnCancelClicked));
            }
        }

        public void CloseDialogWithResult(Window dialog, bool result)
        {
            if (dialog != null)
                dialog.DialogResult = result;
        }

        protected void OnYesClicked(object parameter)
        {
            CloseDialogWithResult(parameter as Window, true);
        }
        protected void OnNoClicked(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
        protected void OnCancelClicked(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
    }
}
