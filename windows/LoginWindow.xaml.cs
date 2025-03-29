using FurniManager.Data;
using FurniManager.Models;
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
using System.Windows.Shapes;

namespace FurniManager.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var email = UsernameBox.Text;
            var password = PassBox.Password;

            bool r = ApplicationDbContext.CheckLogin(email, password);

            if (r)
            {
                using var db = new ApplicationDbContext();
                User user = db.Users.First(user => user.Email == email);
                GlobalContext.Instance.CurrentUser = user;
                MainWindow main = new MainWindow();
                Application.Current.MainWindow = main; // Gán lại cửa sổ chính
                // Application.Current.context. 
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thông tin đăng nhập sai");
            }
        }
    }
}
