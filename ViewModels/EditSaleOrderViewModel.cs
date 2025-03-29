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
        
        Products = db.Products.ToList();

        SaveSaleOrderCommand = new RelayCommand(SaveSaleOrder);

        ExportInvoiceCommand = new RelayCommand(ExportInvoice);
    }

    private void ExportInvoice()
    {
        Console.WriteLine(SaleOrder);
        InvoiceGenerator.ExportSaleOrderToPdf(SaleOrder);
    }



    private void SaveSaleOrder()
    {
        using (var db = new ApplicationDbContext())
        {

            db.SaleOrders.Update(_saleOrder);
            db.SaveChanges();
            MessageBox.Show("Lưu Thành công");
        }
    }
}
