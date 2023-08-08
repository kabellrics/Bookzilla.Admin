using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class PublicationListPage : Page
{
    public PublicationListPage(PublicationListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
