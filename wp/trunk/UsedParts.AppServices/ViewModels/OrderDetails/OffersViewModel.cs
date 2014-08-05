using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UsedParts.AppServices;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.AppServices.ViewModels.Entities;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.UI.ViewModels.OrderDetails
{
    public class OffersViewModel : ClientScreen
    {
        public int OrderId { get; set; }

        public const string IsNeedToRefreshKey = "OffersViewModel_IsNeedToRefresh";

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

        public IList<OfferViewModel> Items { get; set; }

        public new string Title
        {
            get { return Resources.OffersTitle; }
        }

        private readonly IDictionaries _dictionaries;

        public OffersViewModel(
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client,
            IDictionaries dictionaries)
            : base(navigationService, notificationService, client)
        {
            _dictionaries = dictionaries;
        }

        public void NavigateToOfferDetails(OfferViewModel item)
        {
            // nothing
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Items == null || 
                TransientStorage.Contains(IsNeedToRefreshKey) && TransientStorage.Get<bool>(IsNeedToRefreshKey))
                LoadItems();
        }

        private void LoadItems()
        {
            ServiceAction(async () =>
            {
                var items = await Client.GetRequestOffers(OrderId);
                IsNoData = items == null || !items.Any();
                if (items != null)
                {
                    var itemViewModels = new List<OfferViewModel>();
                    foreach (var item in items)
                        itemViewModels.Add(new OfferViewModel(item, _dictionaries));
                    Items = itemViewModels;
                    NotifyOfPropertyChange(() => Items);
                }
            });

        }

    }
}
