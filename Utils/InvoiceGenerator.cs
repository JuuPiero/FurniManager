using FurniManager.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FurniManager.Utils;
public class InvoiceGenerator
{
    public static void ExportSaleOrderToPdf(SaleOrder order)
    {
        string exePath = AppDomain.CurrentDomain.BaseDirectory;
        string pdfPath = Path.Combine(exePath, $"HoaDon_{order.Id}.pdf");

        using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
        {
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "robotoregular.ttf");
            //string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialuni.ttf");
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font fontTitle = new Font(bf, 18, Font.BOLD);
            Font fontText = new Font(bf, 12, Font.NORMAL);

            Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG", fontTitle);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            document.Add(new Paragraph($"Mã đơn hàng: {order.Id}", fontText));
            document.Add(new Paragraph($"Khách hàng: {order.CustomerName}", fontText));
            document.Add(new Paragraph($"SĐT: {order.CustomerPhone}", fontText));
            document.Add(new Paragraph($"Địa chỉ: {order.CustomerAddress}", fontText));
            document.Add(new Paragraph($"Ngày tạo: {order.CreatedAt:dd/MM/yyyy HH:mm}", fontText));
            document.Add(new Paragraph($"Ghi chú: {order.Note}", fontText));
            document.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 10, 40, 15, 20 });

            table.AddCell(new PdfPCell(new Phrase("STT", fontText)));
            table.AddCell(new PdfPCell(new Phrase("Tên sản phẩm", fontText)));
            table.AddCell(new PdfPCell(new Phrase("Số lượng", fontText)));
            table.AddCell(new PdfPCell(new Phrase("Thành tiền", fontText)));

            int index = 1;
            foreach (var item in order.SaleOrderDetails)
            {
                table.AddCell(new PdfPCell(new Phrase(index.ToString(), fontText)));
                table.AddCell(new PdfPCell(new Phrase(item.Product.Name, fontText)));
                table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), fontText)));
                table.AddCell(new PdfPCell(new Phrase($"{item.TotalPrice} VNĐ", fontText)));
                index++;
            }

            document.Add(table);

            Paragraph total = new Paragraph($"Tổng tiền: {order.TotalAmount:0,0} VND", fontText);
            total.Alignment = Element.ALIGN_RIGHT;
            document.Add(total);

            document.Add(new Paragraph("\nCảm ơn quý khách đã mua hàng!", fontText));

            document.Close();
        }

        MessageBox.Show($"Xuất hóa đơn thành công: {pdfPath}");
    }
}