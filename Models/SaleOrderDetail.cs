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
    public class SaleOrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }
        public int SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        [NotMapped]
        public decimal TotalPrice { get; set; }
    }
}
