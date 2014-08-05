using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.PhoneServices;
using UsedParts.PhoneServices.Impl;
using UsedParts.Services;
using UsedParts.Services.Impl;

namespace UsedParts.BackgroundTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            ITaskAgentManager taskAgentManager = new TileUpdateAgentManager();
            if (taskAgentManager.Name != task.Name)
            {
                NotifyComplete();
                return;
            }

            ISettings settings = new IsoStorageSettings();
            IClient client = new Client(settings);
            ITileManager tileManager = new TileManager();

            var lastId = settings.GetValue<int?>(SettingsConstants.LastLoadedOrderId);
            if (!lastId.HasValue)
            {
                NotifyComplete();
                return;
            }
            var manufacturer = settings.GetValue<Manufacturer>(SettingsConstants.Manufacturer);
            var manufacturerId = manufacturer != null ? manufacturer.Id : 0;

            var unreadCountTask = client.GetUnreadRequestCountByManufacturer(manufacturerId, lastId.Value);
            unreadCountTask.ContinueWith(t =>
            {
                var unreadCount = unreadCountTask.Result;
                tileManager.SetPrimaryCount(unreadCount);
                NotifyComplete();
            });
        }
    }
}