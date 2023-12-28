﻿using FluentValidation;
using ProniaOnion.Application.DTOs.Color;

namespace ProniaOnion.Application.Validators
{
    public class ColorCreateDtoValidator : AbstractValidator<ColorCreateDto>
    {
        public ColorCreateDtoValidator()
        {
            RuleFor(X => X.Name).NotEmpty().MaximumLength(50).MinimumLength(3);
        }
    }
}
