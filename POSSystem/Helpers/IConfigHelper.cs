namespace POSSystem.Helpers
{
    public interface IConfigHelper
    {
        string GetStripeSecretKey();
        string GetGoogleClientId();
        string GetGoogleClientSecret();
        string GetDatabaseHost();
        string GetDatabaseUsername();
        string GetDatabasePassword();
        string GetDatabaseName();
    }
}
