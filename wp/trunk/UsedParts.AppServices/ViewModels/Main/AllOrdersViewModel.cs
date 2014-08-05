using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Main
{
    public class AllOrdersViewModel : OrdersViewModel
    {
        private readonly ITileManager _tileManager;

        public Manufacturer Manufacturer
        {
            get { return Settings.GetValue<Manufacturer>(SettingsConstants.Manufacturer); }
            set { Settings.SetValue(SettingsConstants.Manufacturer, value);  }
        }
        public int Total { get; set; }

        public AllOrdersViewModel(INotificationService notificationService,
            INavigationService navigationService,
            IClient client,
            ISettings settings,
            IFavoritesManager<Order> orderFavorites,
            ITileManager tileManager,
            IDictionaries dictionaries
            ) :
            base(notificationService, navigationService, client, settings, orderFavorites)
        {
            if (Manufacturer == null)
                Manufacturer = dictionaries.AllManufacturer;
            _tileManager = tileManager;
        }

        public new string Title
        {
            get { return Resources.AllOrdersTitle; }
        }

        public override void Reload()
        {
            base.Reload();
            TopText = null;
        }

        protected override Task<OrdersResult> GetOrders(int page)
        {
            var task = Client.GetRequestsByManufacturer(Manufacturer.Id, page);
            task.ContinueWith(t =>
            {
                var result = t.Result;
                Total = result.Total;
                TopText = string.Format(Resources.FilterAndTotalFormat, Total, Manufacturer.Name);
                var lastOrder = result.Items.FirstOrDefault();
                var lastOrderId = lastOrder != null ? lastOrder.Id : 0;
                Settings.SetValue(SettingsConstants.LastLoadedOrderId, lastOrderId);
                _tileManager.SetPrimaryCount(0);
            });
            return task;
        }
    }
}
