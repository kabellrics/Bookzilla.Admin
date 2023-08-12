using Bookzilla.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookzilla.Admin.Views
{
    /// <summary>
    /// Logique d'interaction pour TomeGooglereconcilePage.xaml
    /// </summary>
    public partial class TomeGooglereconcilePage : Page
    {
        public TomeGooglereconcilePage(TomeGooglereconcileViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
