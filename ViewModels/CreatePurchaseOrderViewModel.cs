using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.PurchaseOrder;
using FurniManager.Utils;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FurniManager.ViewModels;
public class CreatePurchaseOrderViewModel : INotifyPropertyChanged
{
    public ICommand SavePurchaseOrderCommand { get; }
    public ICommand AddOrderItemCommand { get; }
    public List<Product> Products { get; } = new();

    private PurchaseOrder _purchaseOrder = new();
    public PurchaseOrder PurchaseOrder
    {
        get => _purchaseOrder;
        set
        {
            _purchaseOrder = value;

            OnPropertyChanged(nameof(PurchaseOrder));
        }
    }

    public CreatePurchaseOrderViewModel()
    {
        PurchaseOrder.UserId = GlobalContext.Instance.CurrentUser.Id;


        AddOrderItemCommand = new RelayCommand(AddOrderItem);
        SavePurchaseOrderCommand = new RelayCommand(SavePurchaseOrder);

        using (var db = new ApplicationDbContext())
        {     
            Products = db.Products.ToList();
        }
        AddOrderItem();
    }

    private void SavePurchaseOrder()
    {
        MessageBoxResult result = MessageBox.Show("Are you sure ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        using(var db = new ApplicationDbContext())
        {
            if(PurchaseOrder.Supplier.IsNullOrEmpty())
            {
                MessageBox.Show("Điền đủ thông tin");
                return;
            }
            db.PurchaseOrders.Add(PurchaseOrder);
            db.SaveChanges();

            // Add quantity here
            foreach (var item in PurchaseOrder.PurchaseOrderDetails)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity; // Cộng thêm số lượng
                    db.Products.Update(product);
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Tạo đơn thành công");
            Navigation.Navigate(new CreatePurchaseOrder());
        }
    }

    private void AddOrderItem()
    {
        PurchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetail());
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}