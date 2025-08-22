using System.Xml.Linq;

namespace TestRender.Models.DTO;

public static class ProductDTO
{
    public record AddProductDTO(string Name, string? Description, decimal Price, int CategoryId);
    public record UpdateProductDTO(int Id, string Name, string? Description, decimal Price, int CategoryId);
    public record ProductDataDTO(int Id, string Name, string? Description, decimal Price, int CategoryId);
}
