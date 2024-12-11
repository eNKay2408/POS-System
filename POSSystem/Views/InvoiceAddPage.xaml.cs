using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Repository;
using POSSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POSSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvoiceAddPage : Page
    {
        public class InvoiceAddPageViewModel: BaseViewModel
        {
            private readonly IEmployeeRepository _employeeRepository;
            private List<Employee> _employees;
            public List<Employee> Employees 
            { 
                get=> _employees; 
                set
                {
                    _employees = value;
                    OnPropertyChanged();
                }
            }

            public InvoiceAddPageViewModel()
            {
                _employeeRepository = new EmployeeRepository();
                Employees = new List<Employee>();

                LoadData();
            }

            private async void LoadData()
            {
                await LoadEmployees();
            }

            private async Task LoadEmployees()
            {
                var employees = await _employeeRepository.GetAllEmployees();
                Employees = employees;
            }
        }
        public InvoiceAddPageViewModel ViewModel { get; set; } = new InvoiceAddPageViewModel();


        public InvoiceAddPage()
        {
            this.InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Save invoice clicked",
                PrimaryButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Discard_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

       
    }

}
