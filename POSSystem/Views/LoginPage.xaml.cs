using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using POSSystem.ViewModels;
using System.Diagnostics;

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

            //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //string theme = localSettings.Values["POSAppTheme"].ToString();
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

                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                string theme = localSettings.Values["POSAppTheme"].ToString();
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
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string theme = localSettings.Values["POSAppTheme"].ToString();
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

        private async void GoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel loginViewModel = (LoginViewModel)this.DataContext;

            try
            {
                string name = await loginViewModel.AuthenticateWithGoogle();

                App.AppMainWindow.Content = new MainPage(name);
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                string theme = localSettings.Values["POSAppTheme"].ToString();
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
            } catch (Exception ex)
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
    }
}
