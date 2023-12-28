

using ProniaOnion.Application.Dtos.Product;

namespace ProniaOnion.Application.Dtos.Categories
{
    public record GetCategoryDto(int Id, string Name, ICollection<IncludeProductDto> Products);
}
