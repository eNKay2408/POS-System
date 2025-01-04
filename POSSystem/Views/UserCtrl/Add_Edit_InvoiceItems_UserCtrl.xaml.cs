using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using POSSystem.Helpers;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using POSSystem.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POSSystem.Views.UserCtrl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Add_Edit_InvoiceItems_UserCtrl : UserControl
    {
        public delegate void InvoiceItemEventHandler(InvoiceItem invoiceItem);
        public event InvoiceItemEventHandler DeleteItemHandler;
        public event InvoiceItemEventHandler UpdateItemHandler;
        public Add_Edit_InvoiceItems_UserCtrl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty InvoiceItemsProperty = DependencyProperty.Register(
            "InvoiceItems",
            typeof(FullObservableCollection<InvoiceItem>),
            typeof(Add_Edit_InvoiceItems_UserCtrl),
            //new PropertyMetadata(null, OnInvoiceItemsChanged)
            new PropertyMetadata(null)
        );

        public FullObservableCollection<InvoiceItem> InvoiceItems
        {
            get { return (FullObservableCollection<InvoiceItem>)GetValue(InvoiceItemsProperty); }
            set { SetValue(InvoiceItemsProperty, value); }
        }

        //private static void OnInvoiceItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as Add_Edit_InvoiceItems_UserCtrl;
        //    control.OnPropertyChanged(nameof(Total));
        //}

        public static readonly DependencyProperty EmployeesProperty = DependencyProperty.Register(
            "Employees",
            typeof(List<Employee>),
            typeof(Add_Edit_InvoiceItems_UserCtrl),
            new PropertyMetadata(null)
        );

        public List<Employee> Employees
        {
            get { return (List<Employee>)GetValue(EmployeesProperty); }
            set { SetValue(EmployeesProperty, value); }
        }

        public static readonly DependencyProperty SelectedEmployeeProperty = DependencyProperty.Register(
            "SelectedEmployee",
            typeof(Employee),
            typeof(Add_Edit_InvoiceItems_UserCtrl),
            new PropertyMetadata(null)
        );

        public Employee SelectedEmployee
        {
            get { return (Employee)GetValue(SelectedEmployeeProperty); }
            set { SetValue(SelectedEmployeeProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty = DependencyProperty.Register(
            "Total",
            typeof(decimal),
            typeof(Add_Edit_InvoiceItems_UserCtrl),
            new PropertyMetadata(0.000m)
        );

        public decimal Total
        {
            get
            {
                return decimal.Round((decimal)GetValue(TotalProperty), 2);
            }
            set { SetValue(TotalProperty, (decimal)value); }
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext as InvoiceItem;
            if(item != null)
            {
                UpdateItemHandler?.Invoke(item);
            }    

        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext as InvoiceItem;
            if (item != null)
            {
                DeleteItemHandler?.Invoke(item);
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var parentFrame = this.Parent as Frame;
            if (parentFrame == null)
            {
                parentFrame = GetParentFrame(this);
            }

            parentFrame?.Navigate(typeof(InvoiceAddItemPage));
        }

        private Frame GetParentFrame(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is Frame))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as Frame;
        }

    }
}
