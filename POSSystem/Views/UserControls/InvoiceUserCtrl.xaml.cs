using Microsoft.UI.Xaml;
using POSSystem.Models;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POSSystem;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class InvoiceUserCtrl : Microsoft.UI.Xaml.Controls.UserControl
{
    public static readonly DependencyProperty InfoProperty = 
        DependencyProperty.Register("Info", 
            typeof(List<Employee>), 
            typeof(InvoiceUserCtrl), new PropertyMetadata(null));

    public List<Employee> Info
    {
        get => (List<Employee>)GetValue(InfoProperty);
        set => SetValue(InfoProperty, value);
    }

    public InvoiceUserCtrl()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }
}
