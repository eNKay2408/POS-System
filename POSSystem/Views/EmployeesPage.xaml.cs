using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using POSSystem.Helpers;

namespace POSSystem.Views
{
    public sealed partial class EmployeesPage : Page
    {
        private EmployeeViewModel ViewModel { get; set; }
        public EmployeesPage()
        {
            this.InitializeComponent();
            ViewModel = new EmployeeViewModel();
            this.DataContext = ViewModel;
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Delete Employee Clicked");
            var button = (Button)sender;
            var employee = (Employee)button.DataContext;

            TextBlock reassureQuestion = new TextBlock
            {
                Text = "Are you sure you want to delete employee '" + employee.Name + "' ?"
            };
            ContentDialog deleteCategoryDialog = new ContentDialog
            {
                Title = "DELETE EMPLOYEE",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                Content = reassureQuestion,
                XamlRoot = this.XamlRoot
            };


            var result = await deleteCategoryDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    await ViewModel.DeleteEmployee(employee.Id);
                    await InvoiceAddPage.ModifyEmployee_Handler(employee);
                }
                catch (Exception ex)
                {
                    await DialogHelper.DisplayErrorDialog(ex.Message);
                }
            }

        }


        private async void UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var employee = (Employee)button.DataContext;
            TextBox updateNameTextBox = new TextBox
            {
                PlaceholderText = "Employee's name",
                Text = employee.Name,
                Margin = new Thickness(0, 0, 0, 10)
            };
            TextBox updateEmailTextBox = new TextBox
            {
                PlaceholderText = "Employee's email",
                Text = employee.Email,
                Margin = new Thickness(0, 0, 0, 10)
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(updateNameTextBox);
            panel.Children.Add(updateEmailTextBox);

            ContentDialog dialog = new ContentDialog
            {
                Title = "Update Employee",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                Content = panel,
                XamlRoot = this.XamlRoot,
            };

            while (true)
            {
                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string newName = updateNameTextBox.Text;
                    string newEmail = updateEmailTextBox.Text;
                    try
                    {
                        await ViewModel.UpdateEmployee(employee.Id, newName, newEmail);

                        await InvoiceAddPage.ModifyEmployee_Handler(employee);
                        break; // Exit the loop if update is successful
                    }
                    catch (ArgumentException ex)
                    {
                        await DialogHelper.DisplayErrorDialog(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        await DialogHelper.DisplayErrorDialog("An unexpected error occurred: " + ex.Message);
                        break; // Exit the loop on unexpected errors
                    }
                }
                else
                {
                    break; // Exit the loop if the user cancels
                }
            }
        }

        // Detail: Title = "ERROR", CloseButtonText = "Ok", XamlRoot = this.XamlRoot
        // Content = contentMessage (parameter passed in)
        
    }
}
