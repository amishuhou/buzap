using System.IO.IsolatedStorage;
using UsedParts.Common;

namespace UsedParts.PhoneServices.Impl
{
    public class IsoStorageSettings : ISettings
    {
        private readonly IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;

        public T GetValue<T>(string key)
        {
            T value;
            var exists = _settings.TryGetValue(key, out value);

            return !exists ? default(T) : value;
        }

        public void SetValue<T>(string key, T newValue)
        {
            _settings[key] = newValue;
            _settings.Save();
        }

        public void ResetValue(string key)
        {
            _settings.Remove(key);
            _settings.Save();
        }
    }
}
