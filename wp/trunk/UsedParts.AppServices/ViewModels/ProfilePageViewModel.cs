using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels
{
    public class ProfilePageViewModel : ClientScreen
    {
        public AccountInfo Profile { get; set; }

        public ProfilePageViewModel(
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client) 
            : base(navigationService, notificationService, client)
        {
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            Load();
        }

        private void Load()
        {
            ServiceAction(async () =>
            {
                Profile = await Client.GetStatus();
                NotifyOfPropertyChange(() => Profile);
            });
        }
    }
}
