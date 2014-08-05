using System.Threading.Tasks;
using Caliburn.Micro;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Main
{
    public class MyOrdersViewModel : OrdersViewModel
    {
        public MyOrdersViewModel(INotificationService notificationService, 
            INavigationService navigationService, 
            IClient client, 
            ISettings settings, 
            IFavoritesManager<Order> orderFavorites) : 
            base(notificationService, navigationService, client, settings, orderFavorites)
        {
        }

        public new string Title
        {
            get { return Resources.MyOrdersTitle; }
        }

        protected override Task<OrdersResult> GetOrders(int page)
        {
            if (Client.IsAuthorized)
                return Client.GetRequestsBySeller(Client.Profile.Id, page);
            var taskCompletion = new TaskCompletionSource<OrdersResult>();
            taskCompletion.SetResult(null);
            return taskCompletion.Task;
        }
    }
}
