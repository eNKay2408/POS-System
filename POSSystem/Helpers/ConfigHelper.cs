using Microsoft.Extensions.Configuration;
using System;

namespace POSSystem.Helpers
{
    public class ConfigHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) 
                .AddJsonFile("appsettings.json"); 

            _configuration = builder.Build();
        }

        public string GetStripeSecretKey()
        {
            return _configuration["Stripe:SecretKey"];
        }

        public string GetGoogleClientId()
        {
            return _configuration["Google:ClientSecret:ClientId"];
        }

        public string GetGoogleClientSecret()
        {
            return _configuration["Google:ClientSecret:ClientSecret"];
        }

        public string GetDatabaseHost()
        {
            return _configuration["Database:Host"];
        }

        public string GetDatabaseUsername()
        {
            return _configuration["Database:Username"];
        }

        public string GetDatabasePassword()
        {
            return _configuration["Database:Password"];
        }

        public string GetDatabaseName()
        {
            return _configuration["Database:Database"];
        }
    }
}
