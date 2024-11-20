using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;

namespace POSSystem.Tests
{
    public partial class UnitTestApp : Application
    {
        public UnitTestApp()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.CreateDefaultUI();

            m_window = new UnitTestAppWindow();
            m_window.Activate();

            UITestMethodAttribute.DispatcherQueue = m_window.DispatcherQueue;

            Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.Run(Environment.CommandLine);
        }

        private Window m_window;
    }
}
