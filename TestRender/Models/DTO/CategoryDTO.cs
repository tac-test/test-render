namespace TestRender.Models.DTO;

public static class CategoryDTO
{
    public record AddCategoryDTO(string Name);
    public record UpdateCategoryDTO(int Id, string Name);
    public record CategoryDTO(int Id, string Name);
}
