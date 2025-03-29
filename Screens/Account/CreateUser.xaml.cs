using FurniManager.Data;
using FurniManager.Models;
using FurniManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;


namespace FurniManager.Screens.Account
{
    
    public partial class CreateUser : Page
    {
        public CreateUser()
        {
            InitializeComponent();
            DataContext= new CreateUserViewModel();
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateUserViewModel vm)
            {
                vm.User.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
