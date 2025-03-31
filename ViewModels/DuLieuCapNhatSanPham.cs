using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using Microsoft.EntityFrameworkCore;
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
    public class EditProductViewModel : INotifyPropertyChanged
    {
        public ICommand PickImagesCommand { get; }

        public ICommand SaveProductCommand { get; }

        public ICommand AddAttributeCommand { get; }

        public List<Category> Categories { get => new ApplicationDbContext().Categories.ToList(); }


        private ObservableCollection<ProductImage> _displayImages = new();

        public ObservableCollection<ProductImage> DisplayImages
        {
            get => _displayImages;

            set
            {
                _displayImages = value;
                OnPropertyChanged(nameof(DisplayImages));
            }
        }
        public ObservableCollection<ProductImage> NewImages { get; } = new();




        private Product _product;
        public Product Product
        {
            get => _product;

            set {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }


        public EditProductViewModel(Product product)
        {
            Product = product;
            DisplayImages = Product.Images;

            PickImagesCommand = new RelayCommand(PickImages);
            AddAttributeCommand = new RelayCommand(AddAttribute);

            SaveProductCommand = new RelayCommand(SaveProduct);
        }

        private void PickImages()
        {
            NewImages.Clear();
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
               
                foreach (string filePath in openFileDialog.FileNames)
                {
                    NewImages.Add(new ProductImage { Url = filePath, ProductId = Product.Id });
                }

                if (NewImages.Count > 0)
                {
                    DisplayImages = NewImages;
                }
            }
        }



        private void SaveProduct()
        {
            if (Product.Name.IsNullOrEmpty() || Product.Description.IsNullOrEmpty() || Product.CategoryId <= 0)
            {
                MessageBox.Show("Điền đủ thông tin");

                return;
            }

            using (var db = new ApplicationDbContext())
            {

                if (NewImages.Count > 0)
                {
                    foreach (var image in Product.Images)
                    {
                        image.Delete();
                    }
                    db.Images.RemoveRange(Product.Images);
                    db.SaveChanges();
                }

                foreach (var image in NewImages)
                {
                    image.Store();
                    db.Images.Add(image);
                }
                foreach (var attribute in Product.Attributes.ToList())
                {
                    if (attribute.Value.IsNullOrEmpty() || attribute.Key.IsNullOrEmpty())
                    {
                        if(attribute.Id > 0)
                        {
                            db.Attributes.Remove(attribute);
                            db.SaveChanges();
                        }
                        continue;
                    }
                    db.UpdateOrCreate<ProductAttribute>(attribute);
                }

                db.Entry(Product).State = EntityState.Modified;
                db.Products.Update(Product);
                db.SaveChanges();
                OnPropertyChanged(nameof(Product));
            }
            
            MessageBox.Show("Updated product Successfully");
        }
        private void AddAttribute()
        {
            Product.Attributes.Add(new ProductAttribute { ProductId = Product.Id }); // Thêm một attribute rỗng mới
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
