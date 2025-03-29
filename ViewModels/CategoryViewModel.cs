using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.Categories;
using FurniManager.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FurniManager.ViewModels;
public class CategoryViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand OnDeleteCategory { get; }
    public ICommand OnEditCategory { get;  }

    private ObservableCollection<Category> _categories = new();
    private ObservableCollection<Category> _pagedCategories = new();
    private List<Category> _allCategories = new();
    private Category _selectedCategory;
    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            OnPropertyChanged(nameof(SelectedCategory));
        }
    }
    public ObservableCollection<Category> PagedCategories
    {
        get => _pagedCategories;
        set
        {
            _pagedCategories = value;
            OnPropertyChanged(nameof(PagedCategories));
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
            LoadCategories();
        }
    }

    public int TotalPages => (int)Math.Ceiling((double)_allCategories.Count / PageSize);

    public int PageSize { get; set; } = 10; // Số mục trên mỗi trang

    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    private string _keyword = "";
    public string Keyword
    {
        get => _keyword;
        set
        {
            _keyword = value;
            OnPropertyChanged(nameof(Keyword));
            LoadCategories();
        }
    }
    
    public CategoryViewModel()
    {
        NextPageCommand = new RelayCommand(() => NextPage(), () => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand(() => PreviousPage(), () => CurrentPage > 1);
        OnDeleteCategory = new RelayCommand(() => DeleteCategory());
        OnEditCategory = new RelayCommand(() => EditCategory());
        LoadCategories();
    }
    private void EditCategory()
    {
        if (SelectedCategory != null)
        {
            Navigation.Navigate(new EditCategory(SelectedCategory));
        }
    }
    private void DeleteCategory()
    {
        if(SelectedCategory != null)
        {
            using var db = new ApplicationDbContext();
            db.Categories.Remove(SelectedCategory);
            db.SaveChanges();
            LoadCategories();
        }
    }

    

    private void LoadCategories()
    {
        PagedCategories.Clear();
        using (var db = new ApplicationDbContext())
        {
            var items = db.Categories.Include(c => c.Products).ToList();
            _allCategories = db.Categories.ToList();

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                items = items.Where(c => c.Name.Contains(Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            items = items.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            foreach (var category in items)
            {
                PagedCategories.Add(category);
            }

            if (!PagedCategories.Contains(SelectedCategory))
            {
                SelectedCategory = null;
            }
            OnPropertyChanged(nameof(TotalPages));

            (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

    }

    private void NextPage() => CurrentPage++;
    private void PreviousPage() => CurrentPage--;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}