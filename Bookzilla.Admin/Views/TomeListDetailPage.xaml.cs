using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class TomeListDetailPage : Page
{
    public TomeListDetailPage(TomeListDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
