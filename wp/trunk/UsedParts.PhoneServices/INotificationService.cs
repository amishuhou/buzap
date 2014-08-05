using System;

namespace UsedParts.PhoneServices
{
    public interface INotificationService
    {
        void ShowInfoPopup(string title, string body);
        void ShowDialog(string title, string body, Action okHandler, Action cancelHandler = null);
        bool ShowDialog(string title, string body);

        //void ShowErrorPopup(Exception exception);
    }
}
