using System;

namespace UsedParts.PhoneServices
{
    public interface IPushNotificationManager
    {
        bool IsToastEnabled { get; set; }
        bool IsTileEnabled { get; set; }

        void Connect();
        void Disconnect();

        event EventHandler NotificationReceived;
        event EventHandler<RegisteredEventArgs> Registered;
    }

    public class RegisteredEventArgs : EventArgs
    {
        public string Url { get; set; }
    }

}
