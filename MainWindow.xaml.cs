using FurniManager.Models;
using FurniManager.Screens;
using FurniManager.Screens.Products;
using FurniManager.Screens.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using FurniManager.Screens.Categories;
using FurniManager.Screens.Statistical;
using FurniManager.Screens.PurchaseOrder;
using FurniManager.Screens.SaleOrder;
using FurniManager.Windows;

namespace FurniManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new DashboardPage());
        }

        private void OpenDashboard(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage());
        }

        private void OpenCategory(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListCategories());
        }

      

        private void OpenAccount(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListUser());

        }

        private void OpenProduct(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListProducts());

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
                MainFrame.GoForward();
        }

        public Frame GetNavigation()
        {

            return MainFrame;
        }

        private void OpenStatistical(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StatisticalPage());
        }

        private void OpenPurchaseOrder(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListPurchaseOrder());

        }

        private void OpenSaleOrder(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListSaleOrders());
        }

        private void OpenTest(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TestPage());

        }

       
        private void OnLogout(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
