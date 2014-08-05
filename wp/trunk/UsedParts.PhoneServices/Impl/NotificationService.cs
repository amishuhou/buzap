using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Xna.Framework.GamerServices;
using Action = System.Action;

namespace UsedParts.PhoneServices.Impl
{
    public class NotificationService : INotificationService
    {

        public void ShowInfoPopup(string title, string body)
        {
            Execute.OnUIThread(() => MessageBox.Show(body, title, MessageBoxButton.OK));
        }

        public bool ShowDialog(string title, string body)
        {
            var dialogResult = MessageBox.Show(body, title, MessageBoxButton.OKCancel);

            return dialogResult == MessageBoxResult.OK;
        }

        public void ShowDialog(string title, string body, Action okHandler, Action cancelHandler = null)
        {
            Execute.OnUIThread(
                () => Guide.BeginShowMessageBox(title, body,
                    new List<string> { "Ok", "Cancel" }, 0, MessageBoxIcon.None,
                    asyncResult =>
                    {
                        var result = Guide.EndShowMessageBox(asyncResult);
                        if (result == 0 && okHandler != null)
                            okHandler.OnUIThread();
                        else if (result == 1 && cancelHandler != null)
                            cancelHandler.OnUIThread();
                    }, null));
        }

    }
}
