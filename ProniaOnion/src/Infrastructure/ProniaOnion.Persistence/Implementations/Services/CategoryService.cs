﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.Dtos.Categories;
using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Domain.Entities;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<CategoryItemDto>> GetAllAsync(int page, int take)
        {
            ICollection<Category> categories = await _repository.GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false).ToListAsync();

            ICollection<CategoryItemDto> categoryItemDtos = _mapper.Map<ICollection<CategoryItemDto>>(categories);

            return categoryItemDtos;
        }

        public async Task CreateAsync(CategoryCreateDto CategoryCreateDto)
        {
            bool result = await _repository.IsExistsAsync(c => c.Name == CategoryCreateDto.Name);
            //if (result) 
            if (result) throw new Exception("Category already exist");

            await _repository.AddAsync(_mapper.Map<Category>(CategoryCreateDto));

            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto CategoryUpdateDto)
        {
            Category category = await _repository.GetByIdAsync(id);

            if (category == null) throw new Exception("Not Found");

            _mapper.Map(CategoryUpdateDto, category);

            _repository.Update(category);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id);

            if (category == null) throw new Exception("Not found");

            _repository.Delete(category);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id);
            if (category == null) throw new Exception("Not Found");
            _repository.SoftDelete(category);
            await _repository.SaveChangesAsync();
        }

        public async Task<GetCategoryDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");
            Category item = await _repository.GetByIdAsync(id, includes: nameof(Category.Products));
            if (item is null) throw new Exception("Not Found");

            GetCategoryDto dto = _mapper.Map<GetCategoryDto>(item);

            return dto;
        }

    }
}
