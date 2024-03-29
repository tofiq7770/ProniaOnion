﻿using ProniaOnion.Application.Dtos.Product;

namespace ProniaOnion.Application.Dtos.Tag
{
    public record GetTagDto(int Id, string Name, ICollection<IncludeProductDto> Products);
}
