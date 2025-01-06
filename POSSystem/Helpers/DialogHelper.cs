using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.Helpers
{
    public static class DialogHelper
    {
        public async static Task<ContentDialogResult> DisplayErrorDialog(string contentMessage)
        {
            ContentDialog errorDialog = new()
            {
                Title = "ERROR",
                Content = contentMessage,
                CloseButtonText = "Ok",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            return await errorDialog.ShowAsync();
        }

        public async static Task<ContentDialogResult> DisplayConfirmationDialog(string title,string contentMessage)
        {
            ContentDialog confirmationDialog = new()
            {
                Title = title,
                Content = contentMessage,
                PrimaryButtonText = "Yes",               
                SecondaryButtonText = "No",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            return await confirmationDialog.ShowAsync();
        }
    }
}
