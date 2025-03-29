using FurniManager.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Models
{
    // Phiếu nhập hàng
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        public string Supplier { get; set; } // Nhà cung cấp

        [Precision(18, 4)]
        public decimal TotalAmount { get; set; } // Tổng tiền nhập

        public string Note { get; set; } = "";
        
        public int UserId { get; set; } // = GlobalContext.Instance.CurrentUser.Id; // NHân viên 
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ObservableCollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new();
    }
}
