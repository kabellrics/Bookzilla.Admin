using Windows.UI.Notifications;

namespace Bookzilla.Admin.Contracts.Services;

public interface IToastNotificationsService
{
    public abstract void ShowToastNotification(ToastNotification toastNotification);

    public abstract void ShowToastNotificationSample();
}
