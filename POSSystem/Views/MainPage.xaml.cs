using Microsoft.UI.Xaml.Controls;

namespace POSSystem.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage(string employeeName)
        {
            this.InitializeComponent();

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
                        break;
                }
            }
        }
    }
}
