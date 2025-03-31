using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Models
{
    public class PurchaseOrderDetail
    {
        [Key]
        public int Id { get; set; }


        [Precision(18, 4)]
        public decimal Cost { get; set; } // Giá nhập
        public int Quantity { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }


        [NotMapped]
        public decimal TotalPrice => Cost * Quantity;

    }
}
