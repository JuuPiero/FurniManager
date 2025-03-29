using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.PurchaseOrder;
using FurniManager.Utils;
using Microsoft.EntityFrameworkCore;
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
public class PurchaseOrderViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public ICommand NextPageCommand { get; }
    public ICommand PrevPageCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }


    public int PageSize { get; set; } = 10; // Số mục trên mỗi trang

    public int TotalPages => (int)Math.Ceiling((double)_allPurchaseOrders.Count / PageSize);
    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
            LoadPagedOrder();
        }
    }


    private PurchaseOrder _selectedPurchaseOrder;
    public PurchaseOrder SelectedPurchaseOrder
    {
        get => _selectedPurchaseOrder;
        set
        {
            _selectedPurchaseOrder = value;
            OnPropertyChanged(nameof(SelectedPurchaseOrder));
        }
    }
    private List<PurchaseOrder> _allPurchaseOrders = new();

    private ObservableCollection<PurchaseOrder> _purchaseOrders = new();
    public ObservableCollection<PurchaseOrder> PurchaseOrders
    {
        get => _purchaseOrders;
        set
        {
            _purchaseOrders = value;
            OnPropertyChanged(nameof(PurchaseOrders));
        }
    }


    public PurchaseOrderViewModel()
    {
        NextPageCommand = new RelayCommand(() => NextPage(), () => CurrentPage < TotalPages);
        PrevPageCommand = new RelayCommand(() => PrevPage(), () => CurrentPage > 1);

        EditCommand = new RelayCommand(EditPurchaseOrder);
        DeleteCommand = new RelayCommand(DeletePurchaseOrder);
        LoadPagedOrder();
    }
    private void DeletePurchaseOrder()
    {
        MessageBoxResult result = MessageBox.Show("Are you sure ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.No)
        {
            return;
        }


        if (SelectedPurchaseOrder != null)
        {
            using (var db = new ApplicationDbContext()) { 

                db.PurchaseOrders.Remove(SelectedPurchaseOrder);
                db.SaveChanges();
                LoadPagedOrder();
            }
        }
    }
    private void EditPurchaseOrder()
    {
        if(SelectedPurchaseOrder != null)
        {
            Navigation.Navigate(new EditPurchaseOrder(SelectedPurchaseOrder));
        }
    }

    private void LoadPagedOrder()
    {
        PurchaseOrders.Clear();
        using (var db = new ApplicationDbContext())
        {
            _allPurchaseOrders = db.PurchaseOrders.ToList();
            var items = db.PurchaseOrders.Include(po => po.User).ToList();
            
            items = items.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            foreach (var order in items)
            {
                PurchaseOrders.Add(order);
            }

            if (!PurchaseOrders.Contains(SelectedPurchaseOrder))
            {
                SelectedPurchaseOrder = null;
            }

            OnPropertyChanged(nameof(TotalPages));
            (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PrevPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    private void NextPage() => CurrentPage++;
    private void PrevPage() => CurrentPage--;
}
