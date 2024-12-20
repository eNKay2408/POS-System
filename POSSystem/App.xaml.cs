using Microsoft.UI.Xaml;
using POSSystem.Views;
using POSSystem.Services;
using POSSystem.Repositories;

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

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["POSAppTheme"] == null)
            {
                if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                {
                    localSettings.Values["POSAppTheme"] = "dark";
                }
                else if(Application.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    localSettings.Values["POSAppTheme"] = "light";
                }
                else
                {
                    //default to dark theme (in case user's system theme is not light nor dark)
                    localSettings.Values["POSAppTheme"] = "dark";
                }
            }

            AppMainWindow = new MainWindow();
            LoginPage LoginPage = new LoginPage();
            AppMainWindow.Content = LoginPage;

            var frameworkElement = AppMainWindow.Content as FrameworkElement;
            string theme = localSettings.Values["POSAppTheme"].ToString();
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
