using FluentValidation;
using ProniaOnion.Application.DTOs.Products;

namespace ProniaOnion.Application.Validators
{
    public class ProductUpdateDtoValidator:AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Should be Entered").MaximumLength(50).WithMessage("Name cant be more than 50 characthers").MinimumLength(3).WithMessage("Name at least sholud contain 3 characters");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200).WithMessage("Description cant be more than 200 characthers");
            RuleFor(x => x.SKU).NotEmpty().MaximumLength(25).WithMessage("SKU shouldnt be more than 25 characthers");
            RuleFor(x => x.Price).NotEmpty().LessThanOrEqualTo(999999.99m);
            RuleFor(x => x.CategoryId).Must(c => c > 0);
            RuleForEach(x => x.ColorIds).Must(c => c > 0);
            RuleFor(x => x.ColorIds).NotEmpty();
        }
    }
}
