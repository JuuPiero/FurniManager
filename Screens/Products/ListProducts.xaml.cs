using FurniManager.Utils;
using FurniManager.ViewModels;
using Microsoft.IdentityModel.Tokens;
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

namespace FurniManager.Screens.Products
{
    /// <summary>
    /// Interaction logic for ListProducts.xaml
    /// </summary>
    public partial class ListProducts : Page
    {
        public ListProducts()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();
        }
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!e.Text.IsNullOrEmpty())
                e.Handled = !int.TryParse(e.Text, out _);
        }
        private void OpenCreateProduct(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate(new CreateProduct());
        }
    }
}
