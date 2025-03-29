using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public int PageSize { get; set; } = 10; // Số mục trên mỗi trang

    public int TotalPages => (int)Math.Ceiling((double)_purchaseOrders.Count / PageSize);
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
    private ObservableCollection<PurchaseOrder> PurchaseOrders
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

        LoadPagedOrder();
    }


    private void LoadPagedOrder()
    {
        using (var db = new ApplicationDbContext())
        {
            _allPurchaseOrders = db.PurchaseOrders.ToList();
            var items = db.PurchaseOrders.ToList();
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
