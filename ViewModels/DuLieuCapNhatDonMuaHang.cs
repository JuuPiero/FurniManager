using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using FurniManager.Utils;

namespace FurniManager.ViewModels;
public class EditSaleOrderViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ICommand SaveSaleOrderCommand { get; }

    public ICommand ExportInvoiceCommand { get;  }


    private SaleOrder _saleOrder;
    public SaleOrder SaleOrder
    {
        get => _saleOrder;
        set
        {
            _saleOrder = value;
            OnPropertyChanged(nameof(SaleOrder));
        }
    }

    private decimal _totalAmount;
    public decimal TotalAmount
    {
        get => _totalAmount;
        set { _totalAmount = value; OnPropertyChanged(nameof(TotalAmount)); }
    }

    private float? _discountPercent;
    public float? DiscountPercent
    {
        get => _discountPercent;
        set { _discountPercent = value; RecalculateTotal(); OnPropertyChanged(nameof(DiscountPercent)); OnPropertyChanged(nameof(TotalAmount)); }
    }


    public List<Product> Products { get; set; }

    public List<string> Status { get; set; } = new() {
        "Pending",
        "Completed",
        "Cancel"
    };
    public EditSaleOrderViewModel(SaleOrder saleOrder)
    {

        var db = ApplicationDbContext.Instance;
        _saleOrder = db.SaleOrders.Include(so => so.SaleOrderDetails).ThenInclude(sod => sod.Product).First(so => so.Id == saleOrder.Id);
        foreach (var detail in _saleOrder.SaleOrderDetails)
        {
            if (detail.UnitPrice == 0 && detail.Product != null)
            {
                detail.UnitPrice = detail.Product.Price;
            }
        }
        TotalAmount = _saleOrder.TotalAmount;
        _discountPercent = _saleOrder.DiscountPercent;

        Products = db.Products.ToList();

        SaveSaleOrderCommand = new RelayCommand(SaveSaleOrder);
        ExportInvoiceCommand = new RelayCommand(ExportInvoice);
    }
    private void RecalculateTotal()
    {
        
        TotalAmount = _saleOrder.SaleOrderDetails.Sum(d => d.Quantity * d.UnitPrice);
        if (DiscountPercent != null)
        {
            TotalAmount -= TotalAmount * (decimal)(DiscountPercent / 100);
        }
    }

    private void ExportInvoice()
    {
        InvoiceGenerator.ExportSaleOrderToPdf(SaleOrder);
    }



    private void SaveSaleOrder()
    {
        using (var db = new ApplicationDbContext())
        {
            _saleOrder.DiscountPercent = DiscountPercent;
            _saleOrder.TotalAmount = TotalAmount;
            db.SaleOrders.Update(_saleOrder);
            db.SaveChanges();
            MessageBox.Show("Lưu Thành công");
        }
    }
}
