using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class CollectionListDetailPage : Page
{
    public CollectionListDetailPage(CollectionListDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
