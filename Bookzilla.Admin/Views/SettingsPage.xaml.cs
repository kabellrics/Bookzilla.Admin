using System.Windows.Controls;

using Bookzilla.Admin.ViewModels;

namespace Bookzilla.Admin.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
