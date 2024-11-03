using Microsoft.UI.Xaml;
using POSSystem.Views;

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
            AppMainWindow = new MainWindow();
            LoginPage LoginPage = new LoginPage();
            AppMainWindow.Content = LoginPage;
            AppMainWindow.Activate();
        }
    }
}
