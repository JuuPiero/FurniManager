using System.ComponentModel.DataAnnotations;

namespace FurniManager.Models;


public class ProductAttribute
{
    [Key]
    public int Id { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

}