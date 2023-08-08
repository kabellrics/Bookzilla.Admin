using System.Windows.Controls;

namespace Bookzilla.Admin.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
