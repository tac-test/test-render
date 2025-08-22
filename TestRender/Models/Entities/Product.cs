using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestRender.Models.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(100), Unicode]
    public string Name { get; set; }
    [StringLength(2000), Unicode]
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
