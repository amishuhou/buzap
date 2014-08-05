using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Common;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels
{
    public class SettingsPageViewModel : ClientScreen
    {
        public SettingsPageViewModel(
            INavigationService navigationService, 
            INotificationService notificationService, 
            IClient client, 
            ISettings settings, 
            IPushNotificationManager pushNotificationManager) : 
            base(navigationService, notificationService, client)
        {
            _settings = settings;
            _pushNotificationManager = pushNotificationManager;
        }

        private readonly ISettings _settings;
        private readonly IPushNotificationManager _pushNotificationManager;

        public bool IsNotificationsEnabled
        {
            get { return _settings.GetValue<bool>(SettingsConstants.IsToastEnabled); }
            set
            {
                var oldValue = IsNotificationsEnabled;
                _settings.SetValue(SettingsConstants.IsToastEnabled, value);
                if (oldValue == value) 
                    return;
                _pushNotificationManager.IsToastEnabled = value;
                if (Client.IsAuthorized)
                    _pushNotificationManager.Connect();
                else
                    _pushNotificationManager.Disconnect();
            }
        }
    }
}
