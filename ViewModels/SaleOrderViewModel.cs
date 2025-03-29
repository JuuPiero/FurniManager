using FurniManager.Commands;
using FurniManager.Data;
using FurniManager.Models;
using FurniManager.Screens.SaleOrder;
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
 public class SaleOrderViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ICommand EditSaleOrderCommand { get; }
    public ICommand DeleteSaleOrderCommand { get; }

    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }



    private string _statusFilter;
    public string StatusFilter
    {
        get => _statusFilter;
        set
        {
            _statusFilter = value;
            OnPropertyChanged(nameof(StatusFilter));
            LoadOrders();

        }
    }
    public List<string> Status { get; } = new() 
    {
        "Pending",
        "Completed",
        "Cancel"
    };

    public List<SaleOrder> _allSaleOrders = new();

    private ObservableCollection<SaleOrder> _saleOrders = new();
    public ObservableCollection<SaleOrder> SaleOrders
    {
        get => _saleOrders;
        set
        {
            _saleOrders = value;
            OnPropertyChanged(nameof(SaleOrders));
        }
    }

    private SaleOrder _selectedSaleOrder;
    public SaleOrder SelectedSaleOrder
    {
        get => _selectedSaleOrder;
        set
        {
            _selectedSaleOrder = value;
            OnPropertyChanged(nameof(SelectedSaleOrder));
        }
    }

    public int PageSize { get; set; } = 10; // Số mục trên mỗi trang

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
            LoadOrders();
        }
    }
    public int TotalPages => (int)Math.Ceiling((double)_allSaleOrders.Count / PageSize);



    public SaleOrderViewModel()
    {
        NextPageCommand = new RelayCommand(() => NextPage(), () => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand(() => PreviousPage(), () => CurrentPage > 1);
        EditSaleOrderCommand = new RelayCommand(EditSaleOrder);
        DeleteSaleOrderCommand = new RelayCommand(DeleteOrder);
        LoadOrders();
    }


    private void LoadOrders()
    {
        SaleOrders.Clear();
        var db = ApplicationDbContext.Instance;
        _allSaleOrders = db.SaleOrders.ToList();

        var items = db.SaleOrders.Include(po => po.User).ToList();
        items = items.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

       
        if (!string.IsNullOrWhiteSpace(StatusFilter))
        {
            items = items.Where(i => i.Status == StatusFilter).ToList();
        }


        foreach (var order in items)
        {
            SaleOrders.Add(order);
        }


        if (!SaleOrders.Contains(SelectedSaleOrder))
        {
            SelectedSaleOrder = null;
        }

        OnPropertyChanged(nameof(TotalPages));
        (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private void DeleteOrder()
    {
        MessageBoxResult result = MessageBox.Show("Are you sure ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.No) return;


        if (SelectedSaleOrder != null)
        {
            var db = ApplicationDbContext.Instance;
            db.SaleOrders.Remove(SelectedSaleOrder);
            db.SaveChanges();
        }
        LoadOrders();
    }

    private void EditSaleOrder()
    {
        if(SelectedSaleOrder != null)
        {
            Navigation.Navigate(new EditSaleOrder(SelectedSaleOrder));
        }
    }

    private void NextPage() => CurrentPage++;
    private void PreviousPage() => CurrentPage--;

}