using FurniManager.Utils;
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

namespace FurniManager.Screens.SaleOrder
{
    /// <summary>
    /// Interaction logic for ListSaleOrders.xaml
    /// </summary>
    public partial class ListSaleOrders : Page
    {
        public ListSaleOrders()
        {
            InitializeComponent();
        }

        private void OpenCreateSaleOrder(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate(new CreateSaleOrder());
        }
    }
}
