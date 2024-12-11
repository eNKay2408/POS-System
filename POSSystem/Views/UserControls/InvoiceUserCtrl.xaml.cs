using Microsoft.UI.Xaml;
using POSSystem.Models;
using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-EmployeesInfo.

namespace POSSystem;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class InvoiceUserCtrl : Microsoft.UI.Xaml.Controls.UserControl
{
    public static readonly DependencyProperty EmployeesInfoProperty = 
        DependencyProperty.Register("EmployeesInfo", 
            typeof(List<Employee>), 
            typeof(InvoiceUserCtrl), new PropertyMetadata(null));

    public static readonly DependencyProperty ProductsInfoProperty =
        DependencyProperty.Register("ProductsInfo",
            typeof(List<Product>),
            typeof(InvoiceUserCtrl), new PropertyMetadata(null));

    public List<Employee> EmployeesInfo
    {
        get => (List<Employee>)GetValue(EmployeesInfoProperty);
        set => SetValue(EmployeesInfoProperty, value);
    }

    public List<Product> ProductsInfo
    {
        get => (List<Product>)GetValue(ProductsInfoProperty);
        set => SetValue(ProductsInfoProperty, value);
    }

    public InvoiceUserCtrl()
    {
        this.InitializeComponent();
    }

    private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.UI.Xaml.Controls.ContentDialog()
        {
            Title = "delete product clicked",
            PrimaryButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();

    }
}
