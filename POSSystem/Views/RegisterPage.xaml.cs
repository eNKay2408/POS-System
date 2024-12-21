using Microsoft.UI.Xaml.Controls;
using POSSystem.ViewModels;
using Microsoft.UI.Xaml;
using System;
using POSSystem.Services;

namespace POSSystem.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            this.DataContext = new RegisterViewModel();
        }

        public async void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterViewModel viewModel = (RegisterViewModel)this.DataContext;

            string name = NameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            try
            {
                await viewModel.SaveEmployee(name, email, password, confirmPassword);

                ContentDialog successDialog = new ContentDialog
                {
                    Title = "SUCCESS",
                    Content = "User registered successfully",
                    CloseButtonText = "OK",
                    XamlRoot = App.AppMainWindow.Content.XamlRoot
                };

                await successDialog.ShowAsync();

                App.AppMainWindow.Content = new LoginPage();
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

        public void Login_Click(object sender, RoutedEventArgs e)
        {
            App.AppMainWindow.Content = new LoginPage();
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
