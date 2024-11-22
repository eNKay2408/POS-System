using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;

namespace POSSystem.Views
{
    public sealed partial class EmployeesPage : Page
    {
        public EmployeeViewModel ViewModel { get; set; }
        public EmployeesPage()
        {
            this.InitializeComponent();
            ViewModel = new EmployeeViewModel();
            this.DataContext = ViewModel;
        }

        private async void AddEmployee_Click(object sender, RoutedEventArgs e)
        {

            TextBox nameTextBox = new TextBox
            {
                PlaceholderText = "Employee Name",
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
            };

            TextBox emailTextBox = new TextBox
            {
                PlaceholderText = "Employee Email",
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
            };

            TextBox passwordTextBox = new TextBox
            {
                PlaceholderText = "Employee Password",
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(nameTextBox);
            panel.Children.Add(emailTextBox);
            panel.Children.Add(passwordTextBox);

            ContentDialog addEmployeeDialog = new ContentDialog
            {
                Title = "NEW EMPLOYEE",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                Content = panel,
                XamlRoot = this.XamlRoot
            };

            var result = await addEmployeeDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string newEmployeeName = nameTextBox.Text;
                string newEmployeeEmail = emailTextBox.Text;
                string newEmployeePassword = passwordTextBox.Text;

                try
                {
                    await ViewModel.AddEmployee(newEmployeeName, newEmployeeEmail, newEmployeePassword);
                }
                catch (Exception ex)
                {
                    DisplayErrorDialog(ex.Message);
                }
            }
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
                }
                catch (Exception ex)
                {
                    DisplayErrorDialog(ex.Message);
                }
            }

        }


        private async void UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var employee = (Employee)button.DataContext;
            TextBox updateNameTextBox = new TextBox
            {
                PlaceholderText = employee.Name,
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
            };
            TextBox updateEmailTextBox = new TextBox
            {
                PlaceholderText = employee.Email,
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
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

            var result = await dialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {
                string newName = updateNameTextBox.Text;
                string newEmail = updateEmailTextBox.Text;
                try
                {
                    await ViewModel.UpdateEmployee(employee.Id, newName, newEmail);
                }
                catch (Exception ex)
                {
                    DisplayErrorDialog(ex.Message);
                }
            }
        }

        // Detail: Title = "ERROR", CloseButtonText = "Ok", XamlRoot = this.XamlRoot
        // Content = contentMessage (parameter passed in)
        private async void DisplayErrorDialog(string contentMessage)
        {
            ContentDialog errorDialog = new()
            {
                Title = "ERROR",
                Content = contentMessage,
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };

            await errorDialog.ShowAsync();
        }
    }
}
