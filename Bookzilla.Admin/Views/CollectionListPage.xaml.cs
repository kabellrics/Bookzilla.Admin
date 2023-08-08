using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class CollectionListPage : Page
{
    public CollectionListPage(CollectionListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
