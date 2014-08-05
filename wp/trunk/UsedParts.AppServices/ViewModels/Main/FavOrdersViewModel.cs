using System.Threading.Tasks;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Entities;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Main
{
    public class FavOrdersViewModel : OrdersViewModel
    {
        public FavOrdersViewModel(INotificationService notificationService,
            INavigationService navigationService,
            IClient client,
            ISettings settings,
            IFavoritesManager<Order> orderFavorites) :
            base(notificationService, navigationService, client, settings, orderFavorites)
        {
        }

        public new string Title
        {
            get { return Resources.FavOrdersTitle; }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Reload();
        }

        protected override Task<OrdersResult> GetOrders(int page)
        {
            var result = new OrdersResult {Pages = 1, Items = OrderFavorites.GetAll()};
            var taskCompletionSource = new TaskCompletionSource<OrdersResult>();
            taskCompletionSource.SetResult(result);
            return taskCompletionSource.Task;
        }

        public override void RemoveFromFavorites(OrderViewModel item)
        {
            base.RemoveFromFavorites(item);
            Reload();
        }
    }
}
