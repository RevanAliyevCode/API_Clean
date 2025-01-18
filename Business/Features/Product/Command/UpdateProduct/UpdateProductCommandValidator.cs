using System;
using FluentValidation;

namespace Business.Features.Product.Command.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommandRequest>
{
    public UpdateProductCommandValidator()
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

        RuleFor(x => x.Stock)
            .NotEmpty().WithMessage("Stock is required")
            .GreaterThan(0).WithMessage("Stock must be greater than 0");
    }
}
