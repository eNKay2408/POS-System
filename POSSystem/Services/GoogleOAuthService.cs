using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Oauth2.v2;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;
using POSSystem.Models;
using POSSystem.Helpers;

namespace POSSystem.Services
{
    public class GoogleOAuthService : IGoogleOAuthService
    {
        private static readonly string[] _scopes = { Oauth2Service.Scope.UserinfoEmail, Oauth2Service.Scope.UserinfoProfile };
        private string _applicationName = "POS System";

        private IConfigHelper _configHelper;

        public GoogleOAuthService()
        {
            _configHelper = ServiceFactory.GetChildOf<IConfigHelper>();
        }

        public GoogleOAuthService(IConfigHelper configHelper)
        {
            _configHelper = configHelper;
        }

        public async Task<Employee> AuthenticateAsync()
        {
            UserCredential credential;

            var clientId = _configHelper.GetGoogleClientId();
            var clientSecret = _configHelper.GetGoogleClientSecret();

            var clientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                _scopes,
                "employee",
                CancellationToken.None,
                new NullDataStore());

            var oauthService = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });

            var employeeInfo = await oauthService.Userinfo.Get().ExecuteAsync();

            return new Employee { Name = employeeInfo.Name, Email = employeeInfo.Email };
        }
    }
}