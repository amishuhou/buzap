using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Microsoft.Phone.Tasks;
using UsedParts.AppServices.ViewModels.Base;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.Services;
using UsedParts.UI.ViewModels.OrderDetails;

namespace UsedParts.AppServices.ViewModels
{
    public class MakeOfferPageViewModel : ClientScreen
    {
        private readonly IFileStorageManager _fileStorageManager;

        // TODO: wrap
        private readonly PhotoChooserTask _photoChooserTask = new PhotoChooserTask();
        private readonly CameraCaptureTask _cameraCaptureTask = new CameraCaptureTask();

        private const int MaxImages = 3;

        private const string ImageTempRootPath = "tmp";

        public int OrderId { get; set; }

        public string Price { get; set; }

        public IList<string> Images { get; set; }

        public bool IsImageAmountReached
        {
            get { return Images.Count == MaxImages; }
        }

        public Item Availability { get; set; }
        public Item Warranty { get; set; }
        public Item Delivery { get; set; }
        public Item Condition { get; set; }

        public IDictionaries Dictionaries { get; private set; }
        //public IEnumerable<Item> Availabilities { get { return Dictionaries.Availabilities; } }
        //public IEnumerable<Item> Warranties { get { return Dictionaries.Warranties; } }
        //public IEnumerable<Item> Deliveries { get { return Dictionaries.Deliveries; } }
        //public IEnumerable<Item> Conditions { get { return Dictionaries.Conditions; } }

        public MakeOfferPageViewModel(
            INotificationService notificationService, 
            INavigationService navigationService,
            IClient client,
            IFileStorageManager fileStorageManager,
            IDictionaries dictionaries) 
            : base (navigationService, notificationService, client)
        {
            _fileStorageManager = fileStorageManager;

            Images = new BindableCollection<string>();
            _photoChooserTask.Completed += OnImageTaskCompleted;
            _cameraCaptureTask.Completed += OnImageTaskCompleted;

            Dictionaries = dictionaries;
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            InitDictionaries();
        }

        public void AddImageFromSaved()
        {
            if (IsImageAmountReached)
                return;
            _photoChooserTask.Show();
        }

        public void AddImageUsingCamera()
        {
            if (IsImageAmountReached)
                return;
            _cameraCaptureTask.Show();
        }

        private void OnImageTaskCompleted(object sender, PhotoResult photoResult)
        {
            if (photoResult.TaskResult != TaskResult.OK)
                return;
            var path = string.Format("{0}/{1}", ImageTempRootPath, Path.GetFileName(photoResult.OriginalFileName));


            try
            {
                var bi = new BitmapImage();
                bi.SetSource(photoResult.ChosenPhoto);
                var wb = new WriteableBitmap(bi);
                using (var ms = new MemoryStream())
                {

                    wb.SaveJpeg(ms, wb.PixelWidth, wb.PixelHeight, 0, 80);
                    ms.Seek(0, SeekOrigin.Begin);
                    _fileStorageManager.SaveFile(ms, path);
                }
            }
            catch (FileStorageException fsEx)
            {
                NotificationService.ShowErrorPopup(fsEx);
                return;
            }
            Images.Add(path);
            NotifyOfPropertyChange(() => IsImageAmountReached);
        }

        public void RemoveImage(string image)
        {
            Images.Remove(image);
            NotifyOfPropertyChange(() => IsImageAmountReached);
            try
            {
                _fileStorageManager.DeleteFile(image);
            }
            catch (FileStorageException)
            {
            }
        }

        public void Submit()
        {
            if (!IsAuthorized)
                return;

            ServiceAction(async () =>
            {
                double price;
                Double.TryParse(Price, out price);
                var offer = new Offer
                {
                    OrderId = OrderId,
                    Price = price,
                    Available = Availability.Id,
                    Delivery = Delivery.Id,
                    Condition = Condition.Id,
                    Warranty = Warranty.Id
                };
                if (Images.Any())
                {
                    offer.Images = Images.ToList();
                }
                await Client.MakeOfferForRequest(offer);

                if (NavigationService.CanGoBack)
                {
                    TransientStorage.Put(MainPageViewModel.IsNeedToRefreshKey, true);
                    TransientStorage.Put(OffersViewModel.IsNeedToRefreshKey, true);
                    NavigationService.GoBack();
                }
            });

        }

        private void InitDictionaries()
        {
            if (Availability == null)
                Availability = Dictionaries.Availabilities.First();
            if (Warranty == null)
                Warranty = Dictionaries.Warranties.First();
            if (Delivery == null)
                Delivery = Dictionaries.Deliveries.First();
            if (Condition == null)
                Condition = Dictionaries.Conditions.First();
        }
    }
}
