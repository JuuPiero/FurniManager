using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Precision(18, 4)]
        public decimal Price { get; set; } //-- Giá bán

        public int Quantity { get; set; } // Stock quantity
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Quan hệ: Một sản phẩm có nhiều thuộc tính
        public ObservableCollection<ProductAttribute> Attributes { get; set; } = new();

        // Quan hệ: Một sản phẩm có nhiều hình ảnh
        public ObservableCollection<ProductImage> Images { get; set; } = new();

        [NotMapped]
        public string Preview
        {
             get
                {
                    var firstImage = Images.FirstOrDefault();
                    if (firstImage == null) 
                        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "default.jpg");
                    return firstImage.TempPath;
                }
        }
    }
}
