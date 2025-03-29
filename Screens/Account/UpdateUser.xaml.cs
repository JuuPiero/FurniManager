using FurniManager.Data;
using FurniManager.Models;
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
    /// <summary>
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Page
    {
        public UpdateUser(User user)
        {
            InitializeComponent();
            DataContext = new UpdateUserViewModel(user);
        }

        private void OnUpdate(object sender, RoutedEventArgs e)
        {
            

            MessageBox.Show("User updated successfully");
        }
    }
}
