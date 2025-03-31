

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;

namespace FurniManager.Models;

public class ProductImage 
{
    [Key]
    public int Id { get; set; }

    public string Url { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }


    public void Store() 
    {
        try
        {
            string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(Url)}";
            string destinationPath = Path.Combine(imagesDirectory, fileName);
            // Sao chép ảnh vào thư mục Images
            File.Copy(Url, destinationPath, true);
            Url = Path.Combine("Images", fileName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Lỗi khi lưu ảnh: {ex.Message}");
        }
    }

    public void Delete() 
    {
        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Url);
        // Kiểm tra file tồn tại trước khi xóa
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
    }

    [NotMapped]
    public string TempPath
    {
        get
        {
            string originalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Url);
            string tempPath = Path.Combine(Path.GetTempPath(), $"temp_{Id}_{Path.GetFileName(Url)}");

            try
            {
                if (!File.Exists(tempPath) || File.GetLastWriteTime(tempPath) < File.GetLastWriteTime(originalPath))
                {
                    File.Copy(originalPath, tempPath, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi sao chép ảnh: {ex.Message}");
            }

            return tempPath;
        }
    }
}