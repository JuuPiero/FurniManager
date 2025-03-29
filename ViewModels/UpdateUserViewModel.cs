using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FurniManager.ViewModels
{
    public class UpdateUserViewModel : INotifyPropertyChanged
    {
        public ICommand SaveUserCommand { get; }
        public List<string> Roles { get; } = new();
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;

                OnPropertyChanged(nameof(User));

            }
        }
        public UpdateUserViewModel(User user)
        {
            _user = user;
            Roles.Add("ADMIN");
            Roles.Add("STAFF");
            SaveUserCommand = new RelayCommand(SaveUser);
        }

        private void SaveUser()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Users.Update(User);
                db.SaveChanges();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
