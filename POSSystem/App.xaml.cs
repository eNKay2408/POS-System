using Microsoft.UI.Xaml;
using POSSystem.Views;
using POSSystem.Services;
using POSSystem.Repositories;
using POSSystem.Helpers;

namespace POSSystem
{
    public partial class App : Application
    {
        public static Window AppMainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();

        } 

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            //register repositories
            ServiceFactory.Register<IBrandRepository, BrandRepository>();
            ServiceFactory.Register<ICategoryRepository, CategoryRepository>();
            ServiceFactory.Register<IProductRepository, ProductRepository>();
            ServiceFactory.Register<IEmployeeRepository, EmployeeRepository>();

            ServiceFactory.Register<ISettingsService, SettingsService>();
            ServiceFactory.Register<IEncryptionService, EncryptionService>();
            ServiceFactory.Register<IGoogleOAuthService, GoogleOAuthService>();
            ServiceFactory.Register<IStripeService, StripeService>();
            ServiceFactory.Register<IUriLauncher, UriLauncher>();
            ServiceFactory.Register<IConfigHelper, ConfigHelper>();

            var settingsService = ServiceFactory.GetChildOf<ISettingsService>();
            var theme = settingsService.Load("POSAppTheme");
            if (theme == null)
            {
                if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                {
                    settingsService.Save("POSAppTheme", "dark");
                }
                else if(Application.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    settingsService.Save("POSAppTheme", "light");
                }
                else
                {
                    //default to dark theme (in case user's system theme is not light nor dark)
                    settingsService.Save("POSAppTheme", "dark");
                }
            }

            AppMainWindow = new MainWindow();
            LoginPage LoginPage = new LoginPage();
            AppMainWindow.Content = LoginPage;

            var frameworkElement = AppMainWindow.Content as FrameworkElement;
            if (theme == "dark")
            {
                frameworkElement.RequestedTheme = ElementTheme.Dark;
            }
            else if (theme == "light")
            {
                frameworkElement.RequestedTheme = ElementTheme.Light;
            }
            AppMainWindow.Activate();
        }
    }
}
