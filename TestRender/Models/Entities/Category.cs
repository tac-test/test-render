using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestRender.Models.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(50), Unicode]
    public string Name { get; set; }
}
