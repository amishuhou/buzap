using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.AppServices.ViewModels.Main;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels
{
    public class MainPageViewModel : ClientConductor<OrdersViewModel>
    {

        public MainPageViewModel(
            AllOrdersViewModel allOrdersViewModel,
            MyOrdersViewModel myOrdersViewModel,
            FavOrdersViewModel favOrdersViewModel,
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client) 
            : base(navigationService, notificationService, client)
        {
            Items.Add(allOrdersViewModel);
            Items.Add(myOrdersViewModel);
            Items.Add(favOrdersViewModel);
            allOrdersViewModel.PropertyChanged += OnChildPropertyChanged;
            myOrdersViewModel.PropertyChanged += OnChildPropertyChanged;
            favOrdersViewModel.PropertyChanged += OnChildPropertyChanged;
        }

        public const string IsNeedToRefreshKey = "MainPageViewModel_IsNeedToRefresh";
        public const string ManufacturerIdKey = "MainPageViewModel_ManufacturerId";

        public bool IsFilter { get; set; }

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsBusy")
                IsBusy = Items.Any(view => view.IsBusy);
        }

        public void Reload()
        {
            ActiveItem.Reload();
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            if (TransientStorage.Contains(IsNeedToRefreshKey) && TransientStorage.Get<bool>(IsNeedToRefreshKey))
                Items.ToList().ForEach(vm => vm.Reload());

            if (!TransientStorage.Contains(ManufacturerIdKey)) 
                return;
            var manufacturer = TransientStorage.Get<Manufacturer>(ManufacturerIdKey);
            var all = (AllOrdersViewModel)Items.First(vm => vm is AllOrdersViewModel);
            all.Manufacturer = manufacturer;
            all.Reload();
        }

        public override void ActivateItem(OrdersViewModel item)
        {
            base.ActivateItem(item);
            IsFilter = item is AllOrdersViewModel;
            NotifyOfPropertyChange(() => IsFilter);
            if (item is FavOrdersViewModel)
                item.Reload();
        }

        public void NavigateToFilter()
        {
            NavigationService.UriFor<ManufacturersPageViewModel>().Navigate();
        }
    }
}
