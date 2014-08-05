namespace UsedParts.Common
{
    public interface ISettings
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T newValue);
        void ResetValue(string key);
    }


}
