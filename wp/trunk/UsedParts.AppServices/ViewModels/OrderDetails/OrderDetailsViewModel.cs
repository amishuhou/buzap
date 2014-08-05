using System;
using Caliburn.Micro;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Domain;
using UsedParts.Localization;
using UsedParts.PhoneServices;
using UsedParts.Services;

namespace UsedParts.UI.ViewModels.OrderDetails
{
    public class OrderDetailsViewModel : BusyScreen
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public string Title
        {
            get { return Resources.OrderDetailsTitle; }
        }

        private readonly IClient _client;


        public OrderDetailsViewModel(
            INavigationService navigationService,
            INotificationService notificationService,
            IClient client)
            : base(notificationService)
        {
            _client = client;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Order == null)
                LoadData();
        }

        private void LoadData()
        {
            ServiceAction(async () =>
            {
                Order = await _client.GetRequest(OrderId);
                NotifyOfPropertyChange(() => Order);
            });

        }

    }
}
