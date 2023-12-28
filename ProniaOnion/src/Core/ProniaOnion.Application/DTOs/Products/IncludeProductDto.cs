namespace ProniaOnion.Application.Dtos.Product
{
    public record IncludeProductDto(int Id, string Name, decimal Price, string SKU, string? Description);
}
