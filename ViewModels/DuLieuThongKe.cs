using FurniManager.Data;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using FurniManager.Utils;
using System.Windows.Input;
using FurniManager.Commands;
using System.Windows;
using FurniManager.Models;

namespace FurniManager.ViewModels
{
    class StatisticalViewModel
    {
        public SeriesCollection ProductCountByCategoryChart { get; set; }
        public List<string> ProductCountLabels { get; set; }


        public ICommand ExportExcelCommand { get; }
        public ICommand ExportRevanueCommand { get; }


        public SeriesCollection StockLevelChart { get; set; }
        public List<string> StockLevelLabels { get; set; }



        public SeriesCollection ProductByDateChart { get; set; }
        public List<string> ProductByDateLabels { get; set; }

        public StatisticalViewModel()
        {

            ExportExcelCommand = new RelayCommand(ExportExcel);
            ExportRevanueCommand = new RelayCommand(ExportRevanue);

            using var db = new ApplicationDbContext();

            // Biểu đồ số lượng sản phẩm theo loại
            var productCountData = db.Products
                .GroupBy(p => p.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();

            ProductCountByCategoryChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Số lượng",
                    Values = new ChartValues<int>(productCountData.Select(d => d.Count))
                }
            };
             ProductCountLabels = productCountData.Select(d => d.Category).ToList();

           


            // Biểu đồ tồn kho sản phẩm
            var stockData = db.Products
                .Select(p => new { Product = p.Name, Stock = p.Quantity })
                .ToList();

            StockLevelChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Tồn kho",
                    Values = new ChartValues<int>(stockData.Select(d => d.Stock))
                }
            };
            StockLevelLabels = stockData.Select(d => d.Product).ToList();


            // Biểu đồ thống kê sản phẩm theo thời gian nhập kho
            var productByDateData = db.Products
                .GroupBy(p => p.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(d => d.Date)
                .ToList();

            ProductByDateChart = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Nhập kho",
                    Values = new ChartValues<int>(productByDateData.Select(d => d.Count))
                }
            };
            ProductByDateLabels = productByDateData.Select(d => d.Date.ToShortDateString()).ToList();

        }


        private void ExportExcel()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory; // Lấy thư mục chứa file .exe
            var excelPath = Path.Combine(exePath, "BaoCao_ThongKe.xlsx");

            var db = new ApplicationDbContext();
            var orders = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails).ToList();
            var products = db.Products.ToList();

            var report = new ExcelReportGenerator();
            report.ExportToExcel(orders, products, excelPath);

           MessageBox.Show($"Xuất báo cáo Excel thành công! File nằm ở: {excelPath}");
        }


        private void ExportRevanue()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory; 
            var excelPath = Path.Combine(exePath, "DoanhThu.xlsx");

            var db = new ApplicationDbContext();
            var orders = db.SaleOrders.Include(so => so.SaleOrderDetails).ThenInclude(sod => sod.Product).ToList();
            var products = db.Products.ToList();

            var report = new ExcelReportGenerator();
            report.ExportRevanue(orders, products, excelPath);

            MessageBox.Show($"Xuất báo cáo Excel thành công! File nằm ở: {excelPath}");
        }
    }


}
