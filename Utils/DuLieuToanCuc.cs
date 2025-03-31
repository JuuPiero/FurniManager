using FurniManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Utils
{
    public class GlobalContext : INotifyPropertyChanged
    {
        private static readonly Lazy<GlobalContext> _instance = new(() => new GlobalContext());
        public static GlobalContext Instance => _instance.Value;

        private User? _currentUser;
        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public bool IsAdmin => CurrentUser?.Role == "ADMIN";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private GlobalContext() { } // Chặn tạo instance bên ngoài
    }
}
