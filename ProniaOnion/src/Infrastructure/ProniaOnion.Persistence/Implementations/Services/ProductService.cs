using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Products;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Persistence.Implementations.Repositories;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ITagRepository _tagRepository;

        public ProductService(IProductRepository repository, ICategoryRepository categoryRepository, IColorRepository colorRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        public async Task<IEnumerable<ProductItemDto>> GetAllAsync(int page, int take)
        {
            IEnumerable<ProductItemDto> dtos = _mapper.Map<IEnumerable<ProductItemDto>>(
               await _repository.GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false).ToArrayAsync()
               );
            return dtos;
        }

        public async Task<ProductGetDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");
            Product product = await _repository.GetByIdAsync(id, includes: nameof(Product.Category));
            if (product == null) throw new Exception("Product was not Found");

            ProductGetDto dto = _mapper.Map<ProductGetDto>(product);

            return dto;
        }

        public async Task CreateAsync(ProductCreateDto productDto)
        {
            bool result = await _repository.IsExistsAsync(p => p.Name == productDto.Name);
            if (result) throw new Exception("Name already exist");
            Product product = _mapper.Map<Product>(productDto);
            product.ProductColors = new List<ProductColor>();

            foreach (var colorId in productDto.ColorIds)
            {
                if (!await _colorRepository.IsExistsAsync(c => c.Id == colorId)) throw new Exception("Color is already exist");
                product.ProductColors.Add(new ProductColor { ColorId = colorId });

            }
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProductUpdateDto dto)
        {
            Product existed = await _repository.GetByIdAsync(id, includes: nameof(Product.ProductColors));
            if (existed is null) throw new Exception("Bad Request");

            if (dto.CategoryId != existed.CategoryId)
            {
                if (!await _categoryRepository.IsExistsAsync(c => c.Id == dto.CategoryId)) throw new Exception("Not Found");
            }
            existed = _mapper.Map(dto, existed);
            existed.ProductColors = existed.ProductColors.Where(pc => dto.ColorIds.Any(colId => pc.ColorId == colId)).ToList();

            foreach (var colorId in dto.ColorIds)
            {
                if (!await _colorRepository.IsExistsAsync(c => c.Id == colorId)) throw new Exception("Color was not Found");
                if (!existed.ProductColors.Any(pc => pc.ColorId == colorId))
                {
                    existed.ProductColors.Add(new ProductColor { ColorId = colorId });
                }
            }
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");
            string[] includes = { $" {nameof(Product.ProductColors)}", $"{nameof(Product.ProductTags)}" };
            Product item = await _repository.GetByIdAsync(id, IsDeleted: true, includes: includes);
            if (item == null) throw new Exception("Not Found");
            _repository.Delete(item);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeLeteAsync(int id)
        {
            Product product = await _repository.GetByIdAsync(id);

            if (product is null) throw new Exception("Not found");

            _repository.SoftDelete(product);

            await _repository.SaveChangesAsync();
        }

        public async Task ReverseSoftDeLeteAsync(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");
            Product item = await _repository.GetByIdAsync(id);
            if (item is null) throw new Exception("Not Found");

            _repository.ReverseSoftDelete(item);

            await _repository.SaveChangesAsync();
        }

    }
}



