using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.SaleOrder;
using FurniManager.Utils;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FurniManager.ViewModels;
public class CreateSaleOrderViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ICommand AddOrderItemCommand { get; }
    public ICommand SaveOrderCommand { get; }

    public List<Product> Products { get; set; }
    public List<string> Status { get; set; } = new() {
        "Pending",
        "Completed",
        "Cancel"
    };


    private SaleOrder _saleOrder = new();

    public SaleOrder SaleOrder
    {
        get => _saleOrder;

        set {
            _saleOrder = value;
            OnPropertyChanged(nameof(SaleOrder));
        }
    }

    public CreateSaleOrderViewModel()
    {
        _saleOrder.UserId = GlobalContext.Instance.CurrentUser.Id;
        using (var db = new ApplicationDbContext())
        {
            Products = db.Products.ToList();
        }

        AddOrderItemCommand = new RelayCommand(AddOrderItem);
        SaveOrderCommand = new RelayCommand(SaveSaleOrder);
    }

    private void AddOrderItem()
    {
        _saleOrder.SaleOrderDetails.Add(new SaleOrderDetail());
    }

    private void SaveSaleOrder()
    {
        if(SaleOrder.CustomerName.IsNullOrEmpty() 
            || SaleOrder.CustomerPhone.IsNullOrEmpty()
            || SaleOrder.CustomerAddress.IsNullOrEmpty()
        )
        {
            MessageBox.Show("Điền đầy đủ thông tin");
            return;
        }


        MessageBoxResult result = MessageBox.Show("Are you sure ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        using (var db = new ApplicationDbContext())
        {

            if(SaleOrder.Status == "Completed")
            {
                // Add quantity here
                foreach (var item in SaleOrder.SaleOrderDetails)
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        if(product.Quantity < item.Quantity)
                        {
                            MessageBox.Show("Không đủ hàng");
                            return;
                        }

                        product.Quantity -= item.Quantity; // Trừ thêm số lượng
                        db.Products.Update(product);
                        db.SaveChanges();
                    }
                }
            }

            Console.WriteLine(_saleOrder);
            db.SaleOrders.Add(_saleOrder);
            db.SaveChanges();

            MessageBox.Show("Tạo đơn thành công");

            Navigation.Navigate(new EditSaleOrder(_saleOrder));
        }
    }
}