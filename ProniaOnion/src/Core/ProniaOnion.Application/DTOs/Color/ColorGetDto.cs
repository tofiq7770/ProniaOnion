using ProniaOnion.Application.Dtos.Product;

namespace ProniaOnion.Application.DTOs.Color
{
    public record ColorGetDto(int Id, string Name, ICollection<IncludeProductDto> Products);
}
