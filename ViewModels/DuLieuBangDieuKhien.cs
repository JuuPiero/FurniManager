using FurniManager.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.ViewModels;
public class DashboardViewModel
{
    public int ProductCount { get; set; }
    public int SaleOrderCount { get; set; }
    public int UserCount { get; set; }


    public SeriesCollection ProductCountByCategoryChart { get; set; }
    public List<string> ProductCountLabels { get; set; }


    public DashboardViewModel()
    {
        using var db = new ApplicationDbContext();

        ProductCount = db.Products.Count();
        SaleOrderCount = db.Products.Count();
        UserCount = db.Users.Count();

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





    }
}