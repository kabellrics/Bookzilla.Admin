using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class TomeListPage : Page
{
    public TomeListPage(TomeListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
