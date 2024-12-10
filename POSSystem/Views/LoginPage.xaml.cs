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
            string name = "John Doe";
            App.AppMainWindow.Content = new MainPage(name);

            //LoginViewModel loginViewModel = (LoginViewModel)this.DataContext;

            //string name = await loginViewModel.Login();

            //if (name != null)
            //{
            //    App.AppMainWindow.Content = new MainPage(name);
            //}
            //else
            //{
            //    ContentDialog contentDialog = new ContentDialog
            //    {
            //        Title = "ERROR",
            //        Content = "Invalid email or password",
            //        CloseButtonText = "OK",
            //        XamlRoot = App.AppMainWindow.Content.XamlRoot
            //    };

            //    await contentDialog.ShowAsync();
            //}
        }

        private void LoadSavedCredentials()
        {
            var viewModel = (LoginViewModel)this.DataContext;

            viewModel.LoadSavedCredentials();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            App.AppMainWindow.Content = new RegisterPage();
        }

        private async void GoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel loginViewModel = (LoginViewModel)this.DataContext;

            try
            {
                string name = await loginViewModel.AuthenticateWithGoogle();

                App.AppMainWindow.Content = new MainPage(name);
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
