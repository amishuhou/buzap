using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Phone.Info;

namespace UsedParts.PhoneServices.Impl
{
    internal static class DeviceUtils
    {
        private const int AnidLength = 32;
        private const int AnidOffset = 2;

        public static string GetDeviceFullName()
        {
            var deviceManufacturer = DeviceExtendedProperties.GetValue("DeviceManufacturer");
            var deviceName = DeviceExtendedProperties.GetValue("DeviceName");

            var name = string.Format("{0} {1} {2} {3} {4}",
                deviceManufacturer,
                deviceName,
                GetDeviceUniqueId(),
                GetWindowsLiveAnonymousId(),
                GetAppAssemblyName());
            return name;
        }

        public static string GetDeviceUniqueId()
        {
            byte[] result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = (byte[])uniqueId;
            return ByteArrayToString(result);
        }

        private static string ByteArrayToString(byte[] ba)
        {
            if (ba == null)
                return null;
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string GetWindowsLiveAnonymousId()
        {
            var result = string.Empty;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (AnidLength + AnidOffset))
                    result = anid.ToString().Substring(AnidOffset, AnidLength);
            }

            return result;
        }

        internal static string GetAppAssemblyName()
        {
            var frames = new StackTrace().GetFrames();
            var initialAssembly = (from f in frames
                                      select f.GetMethod().ReflectedType.Assembly.FullName
                                     ).Distinct().ToList().ElementAt(1);
            initialAssembly = Regex.Replace(initialAssembly, @"\, Version=[\d\.]+", string.Empty);
            initialAssembly = Regex.Replace(initialAssembly, @"\, Culture=[\w\-]+", string.Empty);
            initialAssembly = Regex.Replace(initialAssembly, @"\, PublicKeyToken=[\w\d\-]+", string.Empty);
            return initialAssembly;
        }

        private const int Mb = 1024 * 1024;

        public static bool IsWp7Device
        {
            get
            {
                return Environment.OSVersion.Version.Major == 7;
            }
        }

        public static DeviceSpeed DeviceSpeed
        {
            get
            {
                if (IsWp7Device)
                {
                    try
                    {
                        var result = (long)DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit");
                        if (result <= 100 * Mb) return DeviceSpeed.Slow;
                    }

                    catch (ArgumentOutOfRangeException)
                    {
                        // WP7.1 OS, device is definitely at least 512 MB 
                    }

                    return DeviceSpeed.Normal;
                }

                // WP8
                return DeviceSpeed.Fast;
            }
        }

    }

    public enum DeviceSpeed
    {
        Slow = 0,
        Normal = 1,
        Fast = 2
    }
}
