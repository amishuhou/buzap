using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using UsedParts.AppServices.ViewModels;
using UsedParts.AppServices.ViewModels.Main;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.PhoneServices.Impl;
using UsedParts.Services;
using UsedParts.Services.Impl;
using UsedParts.UI.ViewModels.OrderDetails;

namespace UsedParts.UI
{
    public class Bootstrapper : PhoneBootstrapper
    {
        private PhoneContainer _container;
        private IClient _client;

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new TransitionFrame();
        }

        protected override void Configure()
        {
            _container = new PhoneContainer();
            _container.RegisterPhoneServices(RootFrame);

            RegisterServices();
            
            RegisterViews();

            RegisterConventions();

            Execute.InitializeWithDispatcher();

            // update toast settings
            var settings = (ISettings)_container.GetInstance(typeof(ISettings), null);
            if (settings.GetValue<bool?>(SettingsConstants.IsToastEnabled) == null)
                settings.SetValue(SettingsConstants.IsToastEnabled, true);

            // set service language
            _client = (IClient)_container.GetInstance(typeof(IClient), null);
            _client.SetLang(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

            if (Debugger.IsAttached)
            {
                MetroGridHelper.IsVisible = true;
                MetroGridHelper.Opacity = 0.2;
            }

        }

        private void RegisterViews()
        {
            _container.Singleton<MainPageViewModel>();
            _container.PerRequest<AllOrdersViewModel>();
            _container.PerRequest<MyOrdersViewModel>();
            _container.PerRequest<FavOrdersViewModel>();
            _container.PerRequest<ManufacturersPageViewModel>();

            _container.PerRequest<LoginPageViewModel>();
            _container.PerRequest<RegistrationPageViewModel>();
            _container.PerRequest<ProfilePageViewModel>();

            _container.PerRequest<OffersViewModel>();
            _container.PerRequest<OrderDetailsViewModel>();
            _container.PerRequest<OrderMainPageViewModel>();

            _container.PerRequest<MakeOfferPageViewModel>();

            _container.PerRequest<SettingsPageViewModel>();
        }

        private void RegisterServices()
        {
            _container.Singleton<IClient, Client>();
            _container.Singleton<IDictionaries, Dictionaries>();
            _container.Singleton<INotificationService, NotificationService>();
            _container.Singleton<IFileStorageManager, IsoStoreFileStorageManager>();
            _container.Singleton<ISettings, IsoStorageSettings>();
            _container.Singleton<IFavoritesManager<Order>, FavoritesManager<Order>>();
            _container.Singleton<ITaskAgentManager, TileUpdateAgentManager>();
            _container.Singleton<ITileManager, TileManager>();

            _container.RegisterInstance(typeof(IPushNotificationManager), null,
                          new PushNotificationManager("Pro.BUzap.BY"));
        }

        private void RegisterConventions()
        {
            ConventionManager.AddElementConvention<BindableAppBarButton>(
                Control.IsEnabledProperty, "DataContext", "Click");
            ConventionManager.AddElementConvention<BindableAppBarMenuItem>(
                Control.IsEnabledProperty, "DataContext", "Click");
            //ConventionManager.AddElementConvention<MenuItem>(
            //    ItemsControl.ItemsSourceProperty, "DataContext", "Click");

            ViewModelLocator.AddNamespaceMapping(
                "UsedParts.UI.Views",
                "UsedParts.AppServices.ViewModels",
                "Page");
            ViewLocator.AddNamespaceMapping(
                "UsedParts.AppServices.ViewModels",
                "UsedParts.UI.Views",
                "Page");

            ViewModelLocator.AddNamespaceMapping(
                "UsedParts.UI.Views.Main",
                "UsedParts.AppServices.ViewModels.Main");
            ViewLocator.AddNamespaceMapping(
                "UsedParts.AppServices.ViewModels.Main",
                "UsedParts.UI.Views.Main");

            ViewModelLocator.AddNamespaceMapping(
                "UsedParts.UI.Views.Pages.OrderDetails",
                "UsedParts.AppServices.ViewModels.OrderDetails");
            ViewLocator.AddNamespaceMapping(
                "UsedParts.AppServices.ViewModels.OrderDetails",
                "UsedParts.UI.Views.OrderDetails");

        }

        private void InitBackgroundAgents()
        {
            // background tasks
            var taskAgentManager = IoC.Get<ITaskAgentManager>();
            try
            {
                taskAgentManager.Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void InitPushNotifications()
        {
            var nm = IoC.Get<IPushNotificationManager>();
            var settings = IoC.Get<ISettings>();
            nm.IsToastEnabled = settings.GetValue<bool>(SettingsConstants.IsToastEnabled);
            nm.Registered += (sender, args) =>
            {
                try
                {
                    IoC.Get<IClient>().RegisterDevice(args.Url);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };
            if (_client.IsAuthorized)
                nm.Connect();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            InitPushNotifications();
            InitBackgroundAgents();
        }

        protected override void OnLaunch(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            var code = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            if (code == "ua")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
            base.OnLaunch(sender, e);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            //e.Handled = true;
            base.OnUnhandledException(sender, e);

            // post processing actions
            Execute.OnUIThread(() => ReportAboutError(e));
        }


        private static void ReportAboutError(ApplicationUnhandledExceptionEventArgs e)
        {

            if (MessageBox.Show(
                Resources.Generic_DoYouWantToSendErrorReport, 
                Resources.Generic_UnknownError,
                MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            var emailComposeTask = new EmailComposeTask
            {
                To = "office@cybermall.by; a_mishuhou@yahoo.com",
                Subject = "Pro.BUzap.BY - [WP] [Error]",
                Body = string.Format("Stack trace:\r\n{0}",
                                     e.ExceptionObject)
            };

            emailComposeTask.Show();
        }

    }
}
