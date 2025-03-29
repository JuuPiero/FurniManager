using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.Account;
using FurniManager.Utils;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FurniManager.ViewModels
{
    public class CreateUserViewModel : INotifyPropertyChanged
    {
        public ICommand SaveUserCommand { get; }


        public List<string> Roles { get; } = new()
        {
            "ADMIN",
            "STAFF"
        };

        private User _user = new();
        public User User
        { 
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));    
            }
        }

        public CreateUserViewModel()
        {
            SaveUserCommand = new RelayCommand(SaveUser);
        }


        private void SaveUser()
        {
            if(User.Email.IsNullOrEmpty() || User.Password.IsNullOrEmpty() || User.Password.IsNullOrEmpty() || User.Role.IsNullOrEmpty())
            {
                MessageBox.Show("Nhập đầy đủ thông tin");
                return;
            }


            using(var db = new ApplicationDbContext())
            {
                bool exist = db.Users.Any(u => u.Email == User.Email);
                if(exist)
                {
                    MessageBox.Show("Email đã tồn tại");
                    return;
                }


                db.Users.Add(User);
                db.SaveChanges();

                MessageBox.Show("Tạo tài khoản thành công");
                Navigation.Navigate(new CreateUser());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
