using Microsoft.UI.Xaml;
using POSSystem.Views;
using POSSystem.Services;
using POSSystem.Repositories;
using POSSystem.Helpers;
using System.IO;
using System;
using System.Runtime.InteropServices;

namespace POSSystem
{
    public partial class App : Application
    {
        public const int ICON_SMALL = 0;
        public const int ICON_BIG = 1;
        public const int ICON_SMALL2 = 2;

        public const int WM_GETICON = 0x007F;
        public const int WM_SETICON = 0x0080;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);
        public static Window AppMainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Register repositories
            ServiceFactory.Register<IBrandRepository, BrandRepository>();
            ServiceFactory.Register<ICategoryRepository, CategoryRepository>();
            ServiceFactory.Register<IProductRepository, ProductRepository>();
            ServiceFactory.Register<IEmployeeRepository, EmployeeRepository>();
            ServiceFactory.Register<IInvoiceRepository, InvoiceRepository>();
            ServiceFactory.Register<IInvoiceItemRepository, InvoiceItemRepository>();

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
                // Set the theme based on the current application theme
                if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                {
                    settingsService.Save("POSAppTheme", "dark");
                }
                else if (Application.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    settingsService.Save("POSAppTheme", "light");
                }
                else
                {
                    // Default to dark theme (in case user's system theme is not light nor dark)
                    settingsService.Save("POSAppTheme", "dark");
                }

                // Reload the theme after setting it for the first time
                theme = settingsService.Load("POSAppTheme");
            }

            AppMainWindow = new MainWindow();
            LoginPage loginPage = new LoginPage();
            AppMainWindow.Content = loginPage;

            var frameworkElement = AppMainWindow.Content as FrameworkElement;
            if (theme == "dark")
            {
                frameworkElement.RequestedTheme = ElementTheme.Dark;
            }
            else if (theme == "light")
            {
                frameworkElement.RequestedTheme = ElementTheme.Light;
            }

            // Set the window icon
            SetWindowIcon(AppMainWindow, "Assets/logo.ico");

            AppMainWindow.Activate();
        }

        private void SetWindowIcon(Window window, string iconPath)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Prevent ExtractAssociatedIcon from fetching the icon from the default path C:/...
            string fullPath = Path.Combine(AppContext.BaseDirectory, iconPath);
            System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(fullPath);

            // Set both the big and small icons
            SendMessage(hWnd, WM_SETICON, ICON_BIG, ico.Handle);
        }
    }
}
