using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.AppServices.ViewModels.Entities;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Main
{
    public abstract class OrdersViewModel : ClientScreen
    {
        protected OrdersViewModel(
            INotificationService notificationService,
            INavigationService navigationService,
            IClient client,
            ISettings settings,
            IFavoritesManager<Order> orderFavorites)
            : base(navigationService, notificationService, client)
        {
            Settings = settings;
            OrderFavorites = orderFavorites;
        }

        public int? TotalPages { get; set; }

        private string _topText;
        public virtual string TopText
        {
            get { return _topText; }
            set
            {
                _topText = value;
                NotifyOfPropertyChange(() => TopText);
                NotifyOfPropertyChange(() => IsTopText);
            }
        }
        public bool IsTopText { get { return !string.IsNullOrEmpty(TopText); } }

        private int _currentPage;

        protected readonly IFavoritesManager<Order> OrderFavorites;
        private readonly IObservableCollection<OrderViewModel> _items = new BindableCollection<OrderViewModel>();
        protected readonly ISettings Settings;

        private bool _isNoData;
        public bool IsNoData
        {
            get { return _isNoData; }
            set
            {
                _isNoData = value;
                NotifyOfPropertyChange(() => IsNoData);
            }
        }

        public static bool IsNeedToResfresh { get; set; }

        public IObservableCollection<OrderViewModel> Items
        {
            get { return _items; }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (IsNeedToResfresh)
            {
                IsNeedToResfresh = false;
                LoadPage();
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadPage();
        }


        public void NavigateToOffers(OrderViewModel item)
        {
            NavigationService.UriFor<OrderMainPageViewModel>()
                .WithParam(vm => vm.OrderId, item.Id)
                .Navigate();
        }

        public virtual void Reload()
        {
            _currentPage = 0;
            Items.Clear();
            NotifyOfPropertyChange(() => Items);
            LoadPage();
        }

        public void LoadPage()
        {
            if (IsBusy || TotalPages.HasValue && TotalPages <= _currentPage)
                return;

            ServiceAction(async () =>
            {
                var result = await GetOrders(_currentPage);
                if (result == null)
                    return;
                TotalPages = result.Pages;
                var itemViewModels = result.Items.Select(item => new OrderViewModel(item, OrderFavorites)).ToList();
                Items.AddRange(itemViewModels);
                IsNoData = Items == null || !Items.Any();
                _currentPage++;
            });

        }

        protected abstract Task<OrdersResult> GetOrders(int page);

        #region favorites

        public virtual void AddToFavorites(OrderViewModel item)
        {
            OrderFavorites.Add(item.Item);
            Debug.WriteLine("Add {0} {1}", item.Id, item.IsFavorite);
        }

        public virtual void RemoveFromFavorites(OrderViewModel item)
        {
            OrderFavorites.Remove(item.Item);
            Debug.WriteLine("Remove {0} {1}", item.Id, item.IsFavorite);
        }

        #endregion

    }
}
