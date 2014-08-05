using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels
{
    public class LoginPageViewModel : BusyScreen
    {
        private readonly IClient _client;
        private readonly INavigationService _navigationService;
        private readonly IPushNotificationManager _pushNotificationManager;

        public string Email { get; set; }
        public string Password { get; set; }

        public LoginPageViewModel(
            INotificationService notificationService,
            INavigationService navigationService,
            IClient client,
            IPushNotificationManager pushNotificationManager) : base(notificationService)
        {
            _client = client;
            _navigationService = navigationService;
            _pushNotificationManager = pushNotificationManager;
        }

        public void Submit()
        {
            ServiceAction(async () =>
            {
                await _client.Login(Email, Password);

                _pushNotificationManager.Connect();

                if (_navigationService.CanGoBack)
                    _navigationService.GoBack();
            });
        }
    }
}
