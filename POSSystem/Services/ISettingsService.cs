namespace POSSystem.Services
{
    public interface ISettingsService
    {
        void Save(string key, string value);
        string Load(string key);
        void Remove(string key);
    }
}
