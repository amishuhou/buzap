using System;
using System.Collections.Generic;

namespace UsedParts.AppServices
{
    public static class TransientStorage
    {
        private static readonly IDictionary<string, object> Storage = new Dictionary<string, object>();

        public static string Put<T>(T value)
        {
            var key = Guid.NewGuid().ToString();
            Storage[key] = value;
            return key;
        }

        public static string Put<T>(string key, T value)
        {
            Storage[key] = value;
            return key;
        }


        public static T Get<T>(string key)
        {
            if (!Storage.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Unabel to find object by key {0} in transient storage", key));
            }

            var result = Storage[key];
            Storage.Remove(key);
            return (T)result;
        }

        public static bool Contains(string key)
        {
            if (key == null)
                return false;

            return Storage.ContainsKey(key);
        }
    }
}
