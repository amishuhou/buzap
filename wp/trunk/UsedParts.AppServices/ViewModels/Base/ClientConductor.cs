using System;
using Caliburn.Micro;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Base
{
    public abstract class ClientConductor<T> : BusyConductor<T> where T : Screen
    {
        protected readonly IClient Client;
        protected readonly INavigationService NavigationService;

        public bool IsAuthorized
        {
            get { return Client != null && Client.IsAuthorized; }
        }

        public string Title
        {
            get
            {
                if (IsAuthorized && Client.Profile != null)
                    return string.Format("{0} - {1}", Resources.AppTitle, Client.Profile.Name).ToUpperInvariant();
                return Resources.AppTitle.ToUpperInvariant();
            }
        }

        protected ClientConductor(
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client) : base(notificationService)
        {
            Client = client;
            NavigationService = navigationService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            NotifyOfPropertyChange(() => IsAuthorized);
            NotifyOfPropertyChange(() => Title);
        }

        public void NavigateToLogin()
        {
            NavigationService.UriFor<LoginPageViewModel>().Navigate();
        }

        public void NavigateToRegistration()
        {
            NavigationService.UriFor<RegistrationPageViewModel>().Navigate();
        }

        public void NavigateToProfile()
        {
            NavigationService.UriFor<ProfilePageViewModel>().Navigate();
        }

        public void NavigateToSettings()
        {
            NavigationService.UriFor<SettingsPageViewModel>().Navigate();
        }

        public async void Logout()
        {
            StartJob();

            try
            {
                await Client.Logout();
                NotifyOfPropertyChange(() => IsAuthorized);
                NotifyOfPropertyChange(() => Title);
            }
            catch (RequestInterruptedByOsException)
            {
                Logout();
            }
            catch (Exception ex)
            {
                NotificationService.ShowErrorPopup(ex);
            }
            finally
            {
                EndJob();
            }
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            NotifyOfPropertyChange(() => IsAuthorized);
        }
    }
}
