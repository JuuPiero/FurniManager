

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniManager.Models;

public class Category 
{
    [Key]
    public int Id { get; set;}

    public string Name { get; set;}

    public string Description { get; set;}

    public List<Product> Products { get; set;}

    [NotMapped]
    public int Count => Products.Count;

}