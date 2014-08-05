using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Notification;
using System.Diagnostics;

namespace UsedParts.PhoneServices.Impl
{
    public sealed class PushNotificationManager : IPushNotificationManager
    {
        public event EventHandler NotificationReceived;
        public event EventHandler<RegisteredEventArgs> Registered;

        private HttpNotificationChannel _httpChannel;

        private readonly string _channelName;
        private readonly IEnumerable<string> _tileImageRootUrls;

        public bool IsToastEnabled { get; set; }
        public bool IsTileEnabled { get; set; }

        public PushNotificationManager(
            string channelName = null,
            IEnumerable<string> tileImageRootUrls = null)
        {
            _channelName = channelName;
            _tileImageRootUrls = tileImageRootUrls;
            IsToastEnabled = true;
            IsTileEnabled = true;
        }

        public void Disconnect()
        {
            try
            {
                if (_httpChannel != null)
                    _httpChannel.Close();
            }
            catch(Exception ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Channel error: " + ex.Message));
            }
        }

        public void Connect()
        {
            try
            {
                var channelName = _channelName;
                if (string.IsNullOrEmpty(channelName))
                    channelName = DeviceUtils.GetAppAssemblyName();
                _httpChannel = HttpNotificationChannel.Find(channelName);

                //if (!string.IsNullOrEmpty(_channelName) && _httpChannel == null)
                //{
                //    Deployment.Current.Dispatcher.BeginInvoke(() =>
                //        UpdateStatus(string.Format("Error: can't find channel '{0}'", _channelName)));
                //    return;
                //}

                if (null != _httpChannel)
                {
                    SubscribeToChannelEvents();
                    SubscribeToService();
                    SubscribeToNotifications();
                    Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Channel recovered"));
                }
                else
                {
                    _httpChannel = new HttpNotificationChannel(channelName/*, DefaultServiceName*/);
                    SubscribeToChannelEvents();
                    _httpChannel.Open();
                    Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Channel open requested"));
                }
            }
            catch (Exception ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Channel error: " + ex.Message));
            }
        }

        private void SubscribeToService()
        {
            OnRegistered(_httpChannel.ChannelUri.ToString());
            //var managementServiceClient = new ManagementServiceClient(
            //    new BasicHttpBinding(),
            //    new EndpointAddress(_serviceRoot));

            //var subscription = new Subscription
            //{
            //    ChannelUri = _httpChannel.ChannelUri.ToString(),
            //    UserName = _deviceName
            //};
            //managementServiceClient.RegisterDeviceCompleted -= OnRegisterDeviceCompleted;
            //managementServiceClient.RegisterDeviceCompleted += OnRegisterDeviceCompleted;
            //managementServiceClient.RegisterDeviceAsync(subscription);
        }

        //private static void OnRegisterDeviceCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        //{
        //    if (asyncCompletedEventArgs.Error == null)
        //        Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Registration succeeded"));
        //    else
        //        Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Registration failed: " + asyncCompletedEventArgs.Error.Message));
        //}


        private void SubscribeToChannelEvents()
        {
            _httpChannel.ChannelUriUpdated += OnHttpChannelChannelUriUpdated;
            _httpChannel.HttpNotificationReceived += OnHttpChannelHttpNotificationReceived;
            _httpChannel.ShellToastNotificationReceived += OnHttpChannelShellToastNotificationReceived;
            _httpChannel.ErrorOccurred += OnHttpChannelErrorOccurred;
        }

        private static void OnHttpChannelErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus(e.Message));
        }

        private void OnHttpChannelShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(OnNotificationReceived);
        }

        private void OnHttpChannelChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            SubscribeToService();
            SubscribeToNotifications();
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateStatus("Channel created successfully"));
        }

        private void OnHttpChannelHttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(OnNotificationReceived);
        }

        private void SubscribeToNotifications()
        {

            try
            {
                if (IsToastEnabled && !_httpChannel.IsShellToastBound)
                {
                    Debug.WriteLine("Bind to Toast");
                    _httpChannel.BindToShellToast();
                }
                if (!IsToastEnabled && _httpChannel.IsShellToastBound)
                {
                    Debug.WriteLine("Unbind to Toast");
                    _httpChannel.UnbindToShellToast();
                }
            }
            catch (InvalidOperationException iopEx)
            {
                Debug.WriteLine(iopEx);
            }

            try
            {
                if (IsTileEnabled && !_httpChannel.IsShellTileBound)
                {
                    if (_tileImageRootUrls != null && _tileImageRootUrls.Any())
                    {
                        var uris = new Collection<Uri>();
                        _tileImageRootUrls.ToList().ForEach(i => uris.Add(new Uri(i)));
                        _httpChannel.BindToShellTile(uris);
                    }
                    else
                        _httpChannel.BindToShellTile();
                }
                if (!IsTileEnabled && _httpChannel.IsShellTileBound)
                    _httpChannel.UnbindToShellTile();
            }
            catch (InvalidOperationException iopEx)
            {
                Debug.WriteLine(iopEx);
            }
        }

        private void OnNotificationReceived()
        {
            if (NotificationReceived != null)
                NotificationReceived(this, EventArgs.Empty);
        }

        private void OnRegistered(string url)
        {
            UpdateStatus(string.Format("Channel URI: {0}", url));
            if (Registered != null)
                Registered(this, new RegisteredEventArgs { Url = url });
        }

        private static void UpdateStatus(string message)
        {
            Debug.WriteLine(message);
        }
    }
}