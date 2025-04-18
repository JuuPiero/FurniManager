using FurniManager.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Utils;
public class ExcelReportGenerator
{
    public void ExportToExcel(List<PurchaseOrder> orders, List<Product> products, string filePath)
    {
        
        ExcelPackage.License.SetNonCommercialPersonal("12345678910jqk");
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Báo cáo thống kê");

            // Tiêu đề cột
            worksheet.Cells[1, 1].Value = "Tổng số đơn hàng";
            worksheet.Cells[1, 2].Value = "Tổng giá trị";
            worksheet.Cells[1, 3].Value = "Tổng số lượng sản phẩm đã nhập";

            worksheet.Cells[2, 1].Value = orders.Count; // Tổng số đơn hàng
            worksheet.Cells[2, 2].Value = orders.Sum(o => o.TotalAmount); // Tổng giá trị đơn hàng
            worksheet.Cells[2, 3].Value = orders.Sum(o => o.PurchaseOrderDetails.Sum(d => d.Quantity)); // Tổng sản phẩm nhập

            worksheet.Cells[4, 1].Value = "Sản phẩm nhập nhiều nhất";
            worksheet.Cells[4, 2].Value = "Số lượng";

            var mostOrderedProduct = products
                .OrderByDescending(p => orders.Sum(o => o.PurchaseOrderDetails.Where(d => d.ProductId == p.Id).Sum(d => d.Quantity)))
                .FirstOrDefault();

            if (mostOrderedProduct != null)
            {
                worksheet.Cells[5, 1].Value = mostOrderedProduct.Name;
                worksheet.Cells[5, 2].Value = orders.Sum(o => o.PurchaseOrderDetails.Where(d => d.ProductId == mostOrderedProduct.Id).Sum(d => d.Quantity));
            }

            // Lưu file
            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }
    }

    public void ExportRevanue(List<SaleOrder> orders, List<Product> products, string filePath)
    {
        ExcelPackage.License.SetNonCommercialPersonal("12345678910jqk");
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Thống kê theo ngày");

            worksheet.Cells[1, 1].Value = "Ngày";
            worksheet.Cells[1, 2].Value = "Tên sản phẩm";
            worksheet.Cells[1, 3].Value = "Số lượng";
            worksheet.Cells[1, 4].Value = "Đơn giá";
            worksheet.Cells[1, 5].Value = "Thành tiền";

            int row = 2;
            decimal grandTotal = 0;

            foreach (var order in orders.OrderBy(o => o.CreatedAt))
            {
                foreach (var detail in order.SaleOrderDetails)
                {
                    var product = products.FirstOrDefault(p => p.Id == detail.ProductId);
                    if (product == null) continue;

                    worksheet.Cells[row, 1].Value = order.CreatedAt.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 2].Value = product.Name;
                    worksheet.Cells[row, 3].Value = detail.Quantity;
                    worksheet.Cells[row, 4].Value = detail.Product?.Price;
                    worksheet.Cells[row, 5].Formula = $"C{row}*D{row}";

                    grandTotal += detail.Quantity * detail.UnitPrice;
                    row++;
                }
            }

            // Dòng tổng cộng
            worksheet.Cells[row, 4].Value = "Tổng cộng:";
            worksheet.Cells[row, 5].Formula = $"SUM(E2:E{row - 1})";
            worksheet.Cells[row, 4, row, 5].Style.Font.Bold = true;

            // Tự căn chỉnh cột
            worksheet.Cells.AutoFitColumns();

            // Lưu file
            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }
    }
}