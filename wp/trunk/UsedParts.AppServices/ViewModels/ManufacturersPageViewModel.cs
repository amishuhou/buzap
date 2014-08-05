using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;
using UsedParts.UI.ViewModels;

namespace UsedParts.AppServices.ViewModels
{
    public class ManufacturersPageViewModel : ClientScreen
    {
        public IList<Manufacturer> Items { get; set; }

        private readonly Manufacturer _allManufacturer;

        public ManufacturersPageViewModel(
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client,
            IDictionaries dictionaries) 
            : base(navigationService, notificationService, client)
        {
            _allManufacturer = dictionaries.AllManufacturer;
        }

        public void NavigateToOrders(Manufacturer manufacturer)
        {
            TransientStorage.Put(MainPageViewModel.ManufacturerIdKey, manufacturer);
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            if (Items == null)
                LoadItems();
        }

        private void LoadItems()
        {
            ServiceAction(async () =>
            {
                var items = await Client.GetManufacturers();
                if (items == null) 
                    return;
                Items = items.ToList();
                Items.Insert(0, _allManufacturer);
                NotifyOfPropertyChange(() => Items);
            });
        }

    }
}
