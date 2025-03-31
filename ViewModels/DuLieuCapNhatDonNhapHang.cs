using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FurniManager.ViewModels;
public class EditPurchaseOrderViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    public ICommand AddOrderItemCommand { get; }
    public ICommand SavePurchaseOrderCommand { get; }



    private PurchaseOrder _purchaseOrder;
    public PurchaseOrder PurchaseOrder
    {
        get => _purchaseOrder;
        set
        {
            _purchaseOrder = value;
            OnPropertyChanged(nameof(PurchaseOrder));
        }
    }


    public List<Product> Products { get; set; }


    public EditPurchaseOrderViewModel(PurchaseOrder purchaseOrder)
    {

        using (var db = new ApplicationDbContext())
        {
            _purchaseOrder = db.PurchaseOrders.
                Include(po => po.PurchaseOrderDetails).
                ThenInclude(pod => pod.Product).First(po => po.Id == purchaseOrder.Id);

            Products = db.Products.ToList();
        }


        AddOrderItemCommand = new RelayCommand(AddOrderItem);
        SavePurchaseOrderCommand = new RelayCommand(SavePurchaseOrder);
    }

    private void AddOrderItem()
    {
        _purchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetail { PurchaseOrderId = _purchaseOrder.Id });
    }

    private void SavePurchaseOrder()
    {
        using (var db = new ApplicationDbContext())
        {
            



            db.PurchaseOrders.Update(_purchaseOrder);
            db.SaveChanges();

            MessageBox.Show("Lưu Thành công");
        }
    }
}