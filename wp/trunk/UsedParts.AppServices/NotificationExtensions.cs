using System;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices
{
    public static class NotificationExtensions
    {
        public static void ShowErrorPopup(this INotificationService service, Exception exception)
        {
            if (exception is ServiceException)
            {
                var message = exception.Message;
                if (string.IsNullOrEmpty(message))
                    message = Resources.Generic_WebError;
                service.ShowInfoPopup(Resources.Generic_ServerBusinessErrorTitle, message);
            }
            //else if (exception is ServerCommunicationException)
            //{
            //    ShowServerCommunicationErrorPopup(exception as ServerCommunicationException);
            //}
            else
            {
                //Debugger.Break();
                service.ShowInfoPopup(Resources.Generic_UnknownServerErrorTitle, Resources.Generic_UnknownServerErrorBody);
            }
        }
    }
}
