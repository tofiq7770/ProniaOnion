using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Color;

namespace ProniaOnion.Application.DTOs.Products
{
    public record ProductGetDto(int Id, string Name, decimal Price, string SKU, string? Description, int CategoryId, IncludeCategoryDto Category, IncludeColorDto Color);
}
