using System.ComponentModel;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;
using UsedParts.UI.ViewModels.OrderDetails;

namespace UsedParts.AppServices.ViewModels
{
    public class OrderMainPageViewModel : ClientConductor<Screen>
    {
        private readonly IFavoritesManager<Order> _orderFavorites;

        public OrderMainPageViewModel(
            OffersViewModel offersViewModel,
            OrderDetailsViewModel orderDetailsView,
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client,
            IFavoritesManager<Order> orderFavorites
            ) : base(navigationService, notificationService, client)
        {
            _offersView = offersViewModel;
            _orderDetailsView = orderDetailsView;
            Items.Add(_orderDetailsView);
            Items.Add(_offersView);
            _orderDetailsView.PropertyChanged += OnChildPropertyChanged;
            _offersView.PropertyChanged += OnChildPropertyChanged;
            _orderFavorites = orderFavorites;
        }

        private readonly OrderDetailsViewModel _orderDetailsView;
        private readonly OffersViewModel _offersView;

        private int _orderId;

        public int OrderId
        {
            get { return _orderId; }
            set
            {
                _orderId = value;
                if (_offersView != null)
                    _offersView.OrderId = OrderId;
                if (_orderDetailsView != null)
                    _orderDetailsView.OrderId = OrderId;
            }
        }

        public bool IsOrderLoaded { get; set; }

        public bool IsFavorite { get { return _orderFavorites.IsFavorite(_orderDetailsView.Order); } }

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsBusy")
                IsBusy = _orderDetailsView.IsBusy || _offersView.IsBusy;
            if (propertyChangedEventArgs.PropertyName == "Order")
            {
                IsOrderLoaded = _orderDetailsView.Order != null;
                NotifyOfPropertyChange(() => IsOrderLoaded);
                NotifyOfPropertyChange(() => IsFavorite);
            }
        }

        public void NavigateToAddOffer()
        {
            NavigationService.UriFor<MakeOfferPageViewModel>()
                .WithParam(vm => vm.OrderId, OrderId)
                .Navigate();
        }

        public void AddToFavorites()
        {
            _orderFavorites.Add(_orderDetailsView.Order);
            NotifyOfPropertyChange(() => IsFavorite);
        }

        public void RemoveFromFavorites()
        {
            _orderFavorites.Remove(_orderDetailsView.Order);
            NotifyOfPropertyChange(() => IsFavorite);
        }

    }
}
