using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Models
{
    public class SaleOrderDetail : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }

        public int SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }
        //public int? ProductId { get; set; }
        public Product? Product { get; set; }
        //public int Quantity { get; set; }


        private int? _productId;
        public int? ProductId
        {
            get => _productId;
            set { _productId = value; OnPropertyChanged(nameof(ProductId)); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }



        [NotMapped]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public decimal TotalPrice => this.Product.Price * Quantity;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
