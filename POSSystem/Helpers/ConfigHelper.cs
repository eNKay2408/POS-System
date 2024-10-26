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
    }
}
