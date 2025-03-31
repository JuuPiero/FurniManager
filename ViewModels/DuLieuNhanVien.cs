using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.Account;
using FurniManager.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FurniManager.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
    


        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        private List<User> _allUsers = new();
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();


        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public int PageSize { get; set; } = 10; // Số mục trên mỗi trang
        private int _currentPage = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                LoadUsers();
            }
        }

        private string _keyword = "";
        public string Keyword
        {
            get => _keyword;
            set
            {
                _keyword = value;
                OnPropertyChanged(nameof(Keyword));
                LoadUsers();
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)_allUsers.Count / PageSize);
     

        public UserViewModel()
        {
           

            LoadUsers();
            EditUserCommand = new RelayCommand(EditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            NextPageCommand = new RelayCommand(NextPage, () => CurrentPage < TotalPages);
            PrevPageCommand = new RelayCommand(PrevPage, () => CurrentPage > 1);
        }

       

        private void NextPage() => CurrentPage++;
        private void PrevPage() => CurrentPage--;

        private void EditUser()
        {
            if(SelectedUser != null)
            {
                Navigation.Navigate(new UpdateUser(SelectedUser));
            }
        }

        private void DeleteUser()
        {

            MessageBoxResult result = MessageBox.Show("Are you sure ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }


            if (SelectedUser != null)
            {
                using var db = new ApplicationDbContext();
                db.Users.Remove(SelectedUser);
                db.SaveChanges();
                LoadUsers();
            }
        }


        private void LoadUsers()
        {
            Users.Clear();
            using (var db = new ApplicationDbContext())
            {
                _allUsers = db.Users.ToList();

                var users = db.Users.ToList();

                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    users = users.Where(c => c.Name.Contains(Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                users = users.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                foreach (var user in users)
                {
                    Users.Add(user);
                }

                if (!Users.Contains(SelectedUser))
                {
                    SelectedUser = null;
                }
                OnPropertyChanged(nameof(TotalPages));

                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (PrevPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

       
       

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}