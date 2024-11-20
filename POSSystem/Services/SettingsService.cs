using Windows.Storage;

namespace POSSystem.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDataContainer _localSettings;

        public SettingsService()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public void Save(string key, string value)
        {
            _localSettings.Values[key] = value;
        }

        public string Load(string key)
        {
            return _localSettings.Values[key]?.ToString();
        }

        public void Remove(string key)
        {
            _localSettings.Values.Remove(key);
        }
    }
}
