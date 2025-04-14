using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.Products;
using FurniManager.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FurniManager.ViewModels
{
    class ProductViewModel : INotifyPropertyChanged
    {

        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }



        private decimal? _minPriceFilter = null;
        private decimal? _maxPriceFilter = null;
        public decimal? MinPriceFilter
        {
            get => _minPriceFilter;
            set
            {
                _minPriceFilter = value;
                OnPropertyChanged(nameof(MinPriceFilter));
                LoadProducts();
            }
        }
        public decimal? MaxPriceFilter
        {
            get => _maxPriceFilter;
            set
            {
                _maxPriceFilter = value;
                OnPropertyChanged(nameof(MaxPriceFilter));
                LoadProducts();
            }
        }

        public List<string> StatusFilters { get; set; } = new List<string>() { "In Stock", "Out Stock" };
        private string? _statusFilter;
        public string? StatusFilter
        {
            get => _statusFilter;
            set
            {
                _statusFilter = value;
                OnPropertyChanged(nameof(StatusFilter));
                LoadProducts();
            }
        }


        public List<Category> Categories { get; set; }
        public Category _categoryFilter;
        public Category CategoryFilter
        {
            get => _categoryFilter;
            set
            {
                _categoryFilter = value;
                OnPropertyChanged(nameof(CategoryFilter));
                LoadProducts();

            }
        }


        public List<Product> _products = new();
        public ObservableCollection<Product> Products { get; } = new();

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
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
                LoadProducts();
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
                LoadProducts();
            }
        }
        public int TotalPages => (int)Math.Ceiling((double)_products.Count / PageSize);


        public ProductViewModel()
        {
            NextPageCommand = new RelayCommand(() => NextPage(), () => CurrentPage < TotalPages);
            PreviousPageCommand = new RelayCommand(() => PreviousPage(), () => CurrentPage > 1);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            EditProductCommand = new RelayCommand(OpenEditProduct);

            LoadProducts();
        }

        private void LoadProducts()
        {
            Products.Clear();
            using(var db = new ApplicationDbContext())
            {
                Categories = db.Categories.ToList();

                _products = db.Products.ToList();
                var items = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Attributes)
                    .Include(p => p.Images).OrderByDescending(c => c.CreatedAt).ToList();

                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    items = items.Where(p => p.Name.Contains(Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                if (MinPriceFilter != null && MaxPriceFilter != null)
                {
                    items = items.Where(p => p.Price >= MinPriceFilter && p.Price <= MaxPriceFilter).ToList();
                }


                if (CategoryFilter != null)
                {
                    items = items.Where(p => p.CategoryId == CategoryFilter.Id).ToList();
                }

                if (!StatusFilter.IsNullOrEmpty())
                {
                    if(StatusFilter == StatusFilters[0])
                    {
                        items = items.Where(p => p.Quantity > 0).ToList();

                    }
                    else
                    {
                        items = items.Where(p => p.Quantity == 0).ToList();
                    }
                }

                items = items.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                foreach (var product in items)
                {
                    Products.Add(product);
                }

                if (!Products.Contains(SelectedProduct))
                {
                    SelectedProduct = null;
                }
                OnPropertyChanged(nameof(TotalPages));

                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }

        }

        private void DeleteProduct()
        {

            MessageBoxResult result = MessageBox.Show("Are you sure ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }
          
            if (SelectedProduct != null)
            {
                using (var db = new ApplicationDbContext())
                {

                    var attributes = db.Attributes.Where(attr => attr.ProductId == SelectedProduct.Id);
                    db.Attributes.RemoveRange(attributes);
                    var images = db.Images.Where(img => img.ProductId == SelectedProduct.Id).ToList();

                    foreach (var image in images)
                    {
                        image.Delete();
                    }
                    db.Images.RemoveRange(images);
                    db.Products.Remove(SelectedProduct);
                    db.SaveChanges();
                    LoadProducts();
                }
            }
        }
        
        private void OpenEditProduct()
        {
            if(SelectedProduct != null)
            {
                Navigation.Navigate(new EditProduct(SelectedProduct));
            }
        }

        private void NextPage() => CurrentPage++;
        private void PreviousPage() => CurrentPage--;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
