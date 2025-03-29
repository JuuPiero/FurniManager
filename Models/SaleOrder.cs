using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Models
{
    public class SaleOrder
    {
        [Key]
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerAddress { get; set; }

        public string Status { get; set; }


        [Precision(18, 4)]
        public decimal TotalAmount { get; set; } //-- Giá nhập


        public int? UserId { get; set; } // Nhân viên tạo đơn
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
