

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniManager.Models;

public class User 
{
    [Key]
    public int Id { get; set;}

    public string Name { get; set;}

    public string Email { get; set;}
    public string Role { get; set;}
    public string Password { get; set;}


    [NotMapped]
    public bool IsAdmin => Role == "ADMIN" || Role == "admin";

}