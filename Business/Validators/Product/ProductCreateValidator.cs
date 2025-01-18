using System;
using Business.DTOs.Product;
using FluentValidation;

namespace Business.Validators.Product;

public class ProductCreateValidator : AbstractValidator<ProductCreateDTO>
{
    public ProductCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name can't be longer than 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10).WithMessage("Description can't be shorter than 10 characters")
            .MaximumLength(500).WithMessage("Description can't be longer than 500 characters");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Picture)
            .NotEmpty().WithMessage("Picture is required")
            .Must(IsImageValid).WithMessage("Picture must be a valid image");

        RuleFor(x => x.Stock)
            .NotEmpty().WithMessage("Stock is required")
            .GreaterThan(0).WithMessage("Stock must be greater than 0");
    }

    private bool IsImageValid(string image)
    {
        string extension = image[..5];

        return extension.ToLower() switch
        {
            "ivbor" or "/9j/4" => true,
            _ => false,
        };
        ;
    }
}
