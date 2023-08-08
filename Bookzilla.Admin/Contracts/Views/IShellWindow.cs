using System.Windows.Controls;

namespace Bookzilla.Admin.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}
