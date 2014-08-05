using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels
{
    public class RegistrationPageViewModel : BusyScreen
    {
        private readonly IClient _client;
        private readonly INavigationService _navigationService;
        private readonly IPushNotificationManager _pushNotificationManager;

        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Item OrganizationType { get; set; }
        public IEnumerable<Item> OrganizationTypes { get; private set; }

        public RegistrationPageViewModel(
            INotificationService notificationService, 
            INavigationService navigationService,
            IClient client,
            IDictionaries dictionaries,
            IPushNotificationManager pushNotificationManager)
            : base(notificationService)
        {
            _client = client;
            _navigationService = navigationService;
            OrganizationTypes = dictionaries.OrganizationTypes;
            OrganizationType = OrganizationTypes.First();

            _pushNotificationManager = pushNotificationManager;
        }

        public void Submit()
        {

            ServiceAction(async () =>
            {
                var info = new AccountInfo
                {
                    Name = Name,
                    Email = Email,
                    Password = Password,
                    Phone = Phone,
                    OrganizationType = OrganizationType.Id
                };
                await _client.Register(info);

                _pushNotificationManager.Connect();

                if (_navigationService.CanGoBack)
                    _navigationService.GoBack();
            });
            
        }        
    }
}
