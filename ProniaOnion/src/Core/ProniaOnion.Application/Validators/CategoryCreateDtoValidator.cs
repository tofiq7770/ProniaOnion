using FluentValidation;
using ProniaOnion.Application.DTOs.Categories;

namespace ProniaOnion.Application.Validators
{
    public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(X => X.Name).NotEmpty().MaximumLength(50).MinimumLength(3);
        }
    }
}
