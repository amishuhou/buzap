using System.Collections.Generic;
using System.Linq;
using UsedParts.Domain;
using UsedParts.Localization;
using UsedParts.Services;

namespace UsedParts.AppServices.ViewModels.Entities
{
    public class OfferViewModel
    {
        //private const string DefaultOfferImageUrl = "/Toolkit.Content/DefaultOfferImage.png";

        public long Id { get; private set; }
        public string SellerName { get; private set; }
        public string SellerLogo { get; private set; }
        public string Price { get; private set; }
        public string Posted { get; private set; }
        public string PrimaryImage { get; private set; }
        public bool IsPrimaryImage { get { return !string.IsNullOrEmpty(PrimaryImage); } }

        public string Details { get; private set; }

        public OfferViewModel(Offer offer, IDictionaries dictionaries)
        {
            Id = offer.Id;
            Posted = offer.Posted.ToShortDateString();
            SellerLogo = offer.SelllerLogo;
            SellerName = offer.SelllerName;
            Price = string.Format("{0} $", offer.Price);
            PrimaryImage = offer.Images != null && offer.Images.Any()
                               ? offer.Images.First()
                               : null;//DefaultOfferImageUrl;

            var details = new List<string>();
            if (offer.Warranty != 0)
                details.Add(string.Format("{0}: {1}", Resources.Warranty, 
                    dictionaries.Warranties.Where(i => i.Id == offer.Warranty)
                    .Select(i => i.Name)
                    .SingleOrDefault()
                    ));
            if (offer.Delivery != 0)
                details.Add(string.Format("{0}: {1}", Resources.Delivery,
                    dictionaries.Deliveries.Where(i => i.Id == offer.Delivery)
                    .Select(i => i.Name)
                    .SingleOrDefault()
                    ));
            if (offer.Condition != 0)
                details.Add(string.Format("{0}: {1}", Resources.Condition,
                    dictionaries.Conditions.Where(i => i.Id == offer.Condition)
                    .Select(i => i.Name)
                    .SingleOrDefault()
                    ));
            if (offer.Available != 0)
                details.Add(string.Format("{0}: {1}", Resources.Availability,
                    dictionaries.Availabilities.Where(i => i.Id == offer.Available)
                    .Select(i => i.Name)
                    .SingleOrDefault()
                    ));
            Details = string.Join("; ", details).ToLowerInvariant();
        }
    }
}
