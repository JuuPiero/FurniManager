using FurniManager.Models;
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

namespace FurniManager.Screens.Account
{
    public partial class ListUser : Page
    {
        public ListUser()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
        }

        private void OpenCreateUser(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate(new CreateUser());
        }

        private void OpenUpdateUser(object sender, RoutedEventArgs e)
        {
            var userVM = DataContext as UserViewModel;

            Navigation.Navigate(new UpdateUser(userVM.SelectedUser));
        }
    }
}
