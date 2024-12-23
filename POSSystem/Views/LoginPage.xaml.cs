using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using POSSystem.ViewModels;
using System.Diagnostics;
using POSSystem.Services;

namespace POSSystem.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.DataContext = new LoginViewModel();

            LoadSavedCredentials();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            //for development purposes
            //string name = "John Doe";

            //App.AppMainWindow.Content = new MainPage(name);

            //var settingsService = ServiceFactory.GetChildOf<ISettingsService>();
            //string theme = settingsService.Load("POSAppTheme");
            //if (App.AppMainWindow?.Content is FrameworkElement frameworkElement)
            //{
            //    if (theme == "dark")
            //    {
            //        frameworkElement.RequestedTheme = ElementTheme.Dark;
            //    }
            //    else if (theme == "light")
            //    {
            //        frameworkElement.RequestedTheme = ElementTheme.Light;
            //    }
            //}


            LoginViewModel loginViewModel = (LoginViewModel)this.DataContext;

            string name = await loginViewModel.Login();

            if (name != null)
            {
                App.AppMainWindow.Content = new MainPage(name);

                var settingsService = ServiceFactory.GetChildOf<ISettingsService>();
                string theme = settingsService.Load("POSAppTheme");
                if (App.AppMainWindow?.Content is FrameworkElement frameworkElement)
                {
                    if (theme == "dark")
                    {
                        frameworkElement.RequestedTheme = ElementTheme.Dark;
                    }
                    else if (theme == "light")
                    {
                        frameworkElement.RequestedTheme = ElementTheme.Light;
                    }
                }

            }
            else
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    Title = "ERROR",
                    Content = "Invalid email or password",
                    CloseButtonText = "OK",
                    XamlRoot = App.AppMainWindow.Content.XamlRoot
                };

                await contentDialog.ShowAsync();
            }
        }

        private async void LoadSavedCredentials()
        {
            var viewModel = (LoginViewModel)this.DataContext;

            await viewModel.LoadSavedCredentials();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            App.AppMainWindow.Content = new RegisterPage();
            setTheme();
        }

        private async void GoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel loginViewModel = (LoginViewModel)this.DataContext;

            try
            {
                string name = await loginViewModel.AuthenticateWithGoogle();

                App.AppMainWindow.Content = new MainPage(name);
                setTheme();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "ERROR",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = App.AppMainWindow.Content.XamlRoot
                };

                await errorDialog.ShowAsync();
            }

        }

        private static void setTheme()
        {
            var settingsService = ServiceFactory.GetChildOf<ISettingsService>();
            string theme = settingsService.Load("POSAppTheme");
            if (App.AppMainWindow?.Content is FrameworkElement frameworkElement)
            {
                if (theme == "dark")
                {
                    frameworkElement.RequestedTheme = ElementTheme.Dark;
                }
                else if (theme == "light")
                {
                    frameworkElement.RequestedTheme = ElementTheme.Light;
                }
            }
        }
    }
}
