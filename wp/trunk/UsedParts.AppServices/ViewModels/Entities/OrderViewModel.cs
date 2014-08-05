using System.ComponentModel;
using System.Linq;
using UsedParts.Domain;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Entities
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        //private const string DefaultOrderImageUrl = "/Toolkit.Content/DefaultOrderImage.png";

        private readonly IFavoritesManager<Order> _favoritesManager;

        public int Id { get { return Item.Id; } }
        public string Name { get { return Item.Name; } }
        public string Posted { get { return Item.Posted.ToShortDateString(); } }
        public string PrimaryImage
        {
            get
            {
                return Item.Images != null && Item.Images.Any()
                           ? Item.Images.First()
                           : null;
            }
        }
        public int OffersCount { get { return Item.OffersCount; } }
        public bool IsOffersAvailable { get { return OffersCount > 0; } }
        public string Body { get { return Item.Body; } }
        public bool IsPrimaryImage { get { return !string.IsNullOrEmpty(PrimaryImage); } }
        public bool IsFavorite { get { return _favoritesManager.IsFavorite(Item); } }
        public Order Item { get; private set; }

        public OrderViewModel(Order order, IFavoritesManager<Order> favoritesManager)
        {
            Item = order;
            _favoritesManager = favoritesManager;
            _favoritesManager.ItemChanged += (sender, args) =>
            {
                if (!args.Item.Equals(Item)) 
                    return;
                OnPropertyChanged("IsFavorite");
            };
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
