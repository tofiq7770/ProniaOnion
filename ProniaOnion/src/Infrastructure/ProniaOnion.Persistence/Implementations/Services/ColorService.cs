using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Color;
using ProniaOnion.Domain.Entities;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _repository;
        private readonly IMapper _mapper;

        public ColorService(IColorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ColorItemDto>> GetAllAsync(int page, int take)
        {
            ICollection<Color> colors = await _repository.GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, IsDeleted: true).ToListAsync();

            ICollection<ColorItemDto> colorItemDtos = new List<ColorItemDto>();

            foreach (Color color in colors)
            {
                colorItemDtos.Add(new ColorItemDto(color.Id, color.Name));
            }
            return colorItemDtos;
        }
        public async Task CreateAsync(ColorCreateDto ColorCreateDto)
        {
            bool result = await _repository.IsExistsAsync(c => c.Name == ColorCreateDto.Name);
            if (result) throw new Exception("Color already exist");
            await _repository.AddAsync(_mapper.Map<Color>(ColorCreateDto));

            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ColorUpdateDto ColorUpdateDto)
        {
            Color color = await _repository.GetByIdAsync(id);

            if (color == null) throw new Exception("Not Found");
            bool result = await _repository.IsExistsAsync(c => c.Name == ColorUpdateDto.Name);
            if (result) throw new Exception("Already exist");

            _mapper.Map(ColorUpdateDto, color);

            _repository.Update(color);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Color color = await _repository.GetByIdAsync(id);

            if (color == null) throw new Exception("Not found");

            _repository.Delete(color);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            Color color = await _repository.GetByIdAsync(id);
            if (color == null) throw new Exception("Not Found");
            _repository.SoftDelete(color);
            await _repository.SaveChangesAsync();
        }

    }
}
