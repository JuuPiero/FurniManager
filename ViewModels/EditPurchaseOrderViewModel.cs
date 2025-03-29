using FurniManager.Data;
using FurniManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.ViewModels;
public class EditPurchaseOrderViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


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
    public EditPurchaseOrderViewModel(PurchaseOrder purchaseOrder)
    {

        using (var db = new ApplicationDbContext())
        {
            _purchaseOrder = db.PurchaseOrders.
                Include(po => po.PurchaseOrderDetails).
                ThenInclude(pod => pod.Product).First(po => po.Id == purchaseOrder.Id);




            //var items = db.PurchaseOrderDetails.Where(pod => pod.PurchaseOrderId == _purchaseOrder.Id).ToList();

            //foreach (var item in items)
            //{
            //    _purchaseOrder.PurchaseOrderDetails.Add(item);
            //}
        }
    }


}