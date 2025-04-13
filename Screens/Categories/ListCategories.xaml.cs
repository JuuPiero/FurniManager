using FurniManager.Utils;
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

namespace FurniManager.Screens.Categories
{
    /// <summary>
    /// Interaction logic for ListCategories.xaml
    /// </summary>
    public partial class ListCategories : Page
    {
        public ListCategories()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel();

        }

        private void OpenCreateCategory(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate(new CreateCategory());
        }
    }
}
