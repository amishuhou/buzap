using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Caliburn.Micro;
using UsedParts.PhoneServices;

namespace UsedParts.AppServices.ViewModels.Base
{

    public abstract class BusyConductor<T> : Conductor<T>.Collection.OneActive, IBusy where T : Screen
    {
        private byte _depth;
        private bool _isBusy;

        protected readonly INotificationService NotificationService;

        protected BusyConductor(INotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public IDisposable StartJob()
        {
            IsBusy = true;

            _depth++;

            return new DisposableSource(EndJob);
        }

        public void EndJob()
        {
            if (!IsBusy)
                throw new InvalidOperationException();

            _depth--;

            if (_depth == 0)
                IsBusy = false;
        }

        protected async void ServiceAction(Func<Task> operation)
        {
            StartJob();

            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Service error: {0}", ex);
                NotificationService.ShowErrorPopup(ex);
            }
            finally
            {
                EndJob();
            }
        }

    }
}
