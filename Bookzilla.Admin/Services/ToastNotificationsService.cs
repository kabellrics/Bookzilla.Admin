using Bookzilla.Admin.Contracts.Services;

using CommunityToolkit.WinUI.Notifications;

using Windows.UI.Notifications;

namespace Bookzilla.Admin.Services;

public partial class ToastNotificationsService : IToastNotificationsService
{
    public ToastNotificationsService()
    {
    }

    public void ShowToastNotification(ToastNotification toastNotification)
    {
        ToastNotificationManagerCompat.CreateToastNotifier().Show(toastNotification);
    }
}
