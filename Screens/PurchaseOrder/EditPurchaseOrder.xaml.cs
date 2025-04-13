using FurniManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FurniManager.Screens.PurchaseOrder;
public partial class EditPurchaseOrder : Page
{
    public EditPurchaseOrder(FurniManager.Models.PurchaseOrder purchaseOrder)
    {
        InitializeComponent();
        DataContext = new EditPurchaseOrderViewModel(purchaseOrder);
    }

    private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !int.TryParse(e.Text, out _);
    }
}