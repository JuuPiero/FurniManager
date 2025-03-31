using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.Products;
using FurniManager.Utils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
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
using System.Xml.Linq;

namespace FurniManager.ViewModels
{
    class CreateProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductAttribute> Attributes { get; set; } = new();
        public List<Category> Categories { get; set; } = new();


        public ObservableCollection<ProductImage> Images { get; set; } = new();

        public ICommand AddAttributeCommand { get; }
        public ICommand PickImagesCommand { get; }
        public ICommand SaveProductCommand { get; }



        private Product _product = new();
        public Product Product 
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public CreateProductViewModel()
        {
            using (var db = new ApplicationDbContext())
            {
                Categories = db.Categories.ToList();
            }

            Attributes.Add(new ProductAttribute());
            AddAttributeCommand = new RelayCommand(AddAttribute);
            PickImagesCommand = new RelayCommand(PickImages);
            SaveProductCommand = new RelayCommand(SaveProduct);
        }
        private void PickImages()
        {

            Images.Clear();
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    // Chỉ lưu đường dẫn tạm, chưa copy file
                    //string fileName = Path.GetFileName(filePath);
                    //string relativePath = $"Images/{fileName}";

                    Images.Add(new ProductImage { Url = filePath });
                }
            }
        }


        private void AddAttribute()
        {
            Attributes.Add(new ProductAttribute()); // Thêm một attribute rỗng mới
        }

        private void SaveProduct()
        {
            if(Product.Name.IsNullOrEmpty() || Product.Description.IsNullOrEmpty() || Product.CategoryId <= 0)
            {
                MessageBox.Show("Điền đủ thông tin");

                return;
            }



            using (var db = new ApplicationDbContext())
            {
                db.Products.Add(Product);
                db.SaveChanges();

                foreach (var attribute in Attributes)
                {
                    if (!attribute.Value.IsNullOrEmpty() && !attribute.Key.IsNullOrEmpty())
                    {
                        attribute.ProductId = Product.Id;
                        db.Attributes.Add(attribute);
                        db.SaveChanges();
                    }
                }

                foreach (var image in Images)
                {
                    image.Store();
                    image.ProductId = Product.Id;
                    db.Images.Add(image);
                    db.SaveChanges();
                }
            }
            MessageBox.Show("Create new Product successfully");
            Navigation.Navigate(new CreateProduct());
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
