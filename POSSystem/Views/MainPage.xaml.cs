using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POSSystem.Services;
using System.Diagnostics;

namespace POSSystem.Views
{
    public sealed partial class MainPage : Page
    {
        private bool isDarkTheme;
        private readonly ISettingsService settingsService = ServiceFactory.GetChildOf<ISettingsService>();

        public MainPage(string employeeName)
        {
            this.InitializeComponent();

            // Determine the current system theme
            if (settingsService.Load("POSAppTheme") == "dark")
            {
                isDarkTheme = true;
            }
            else if (settingsService.Load("POSAppTheme") == "light")
            {
                isDarkTheme = false;
            }

            ChangeThemeBtnContent.Text = isDarkTheme ? "Switch to Light Theme" : "Switch to Dark Theme";

            NavView.SelectedItem = NavView.MenuItems[0];
            NavView.PaneTitle = $"Welcome, {employeeName}";

            ContentFrame.Navigate(typeof(ProductsPage));

            NavView.SelectionChanged += NavView_SelectionChanged;
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "products":
                        ContentFrame.Navigate(typeof(ProductsPage));
                        break;
                    case "categories":
                        ContentFrame.Navigate(typeof(CategoriesPage));
                        break;
                    case "brands":
                        ContentFrame.Navigate(typeof(BrandsPage));
                        break;
                    case "invoices":
                        ContentFrame.Navigate(typeof(InvoicesPage));
                        break;
                    case "employees":
                        ContentFrame.Navigate(typeof(EmployeesPage));
                        break;
                    case "logout":
                        App.AppMainWindow.Content = new LoginPage();
                        if (App.AppMainWindow?.Content is FrameworkElement frameworkElement)
                        {
                            frameworkElement.RequestedTheme = isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
                        }
                        break;
                }
            }
        }

        private void ChangeThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            isDarkTheme = !isDarkTheme;
            if (App.AppMainWindow?.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
            }

            ChangeThemeBtnContent.Text = isDarkTheme ? "Switch to Light Theme" : "Switch to Dark Theme";

            settingsService.Save("POSAppTheme", isDarkTheme ? "dark" : "light");
        }

    }
}
