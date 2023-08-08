using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class PublicationListDetailPage : Page
{
    public PublicationListDetailPage(PublicationListDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
