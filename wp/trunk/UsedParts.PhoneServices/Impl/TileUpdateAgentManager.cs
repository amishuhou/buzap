using System;
using Microsoft.Phone.Scheduler;

namespace UsedParts.PhoneServices.Impl
{
    public class TileUpdateAgentManager : ITaskAgentManager
    {
        public string Name { get { return "Pro.BUzap unread orders updater"; } }

        //private readonly ISettings _appSettings;

        public bool IsRunning { get { return IsTaskRunning(); } }
        public bool CanRun { get;  private set; }

        //public NewsUpdaterAgentManager(ISettings appSettings)
        //{
        //    _appSettings = appSettings;
        //}

        public void Run()
        {
            CanRun = true;

            if (DeviceUtils.DeviceSpeed == DeviceSpeed.Slow)
            {
                return;
            }
            //if (!_appSettings.AutomaticTileUpdate)
            //{
            //    return;
            //}


            var periodicTask = new PeriodicTask(Name)
            {
                //Description = UIStrings.PeriodicTaskAgentDescription
                Description = "Update number of unread orders on main tile / обновление кол-ва непрочитанных заявок на основной иконке"
            };

            Stop();

            try
            {
                ScheduledActionService.Add(periodicTask);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                    CanRun = false;
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                    CanRun = false;
            }

#if DEBUG
            ScheduledActionService.LaunchForTest(periodicTask.Name, TimeSpan.FromSeconds(20));
#endif

        }

        public void Stop()
        {
            if (DeviceUtils.DeviceSpeed == DeviceSpeed.Slow)
            {
                return;
            }
            if (ScheduledActionService.Find(Name) != null)
            {
                ScheduledActionService.Remove(Name);
            }
        }

        private bool IsTaskRunning()
        {
            if (DeviceUtils.DeviceSpeed == DeviceSpeed.Slow)
            {
                return false;
            }
            var task = ScheduledActionService.Find(Name);
            if (task != null)
            {
                return task.IsScheduled;
            }
            return false;
        }
    }
}

