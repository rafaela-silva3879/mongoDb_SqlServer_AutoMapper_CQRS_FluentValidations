
using DeveloperStore.Application.Models.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DeveloperStore.Application.Validation
{
    public class ProductEditModelValidation : AbstractValidator<(string Name, string UnitPrice, string Quantity, string UserId)>
    {
        public ProductEditModelValidation()
        {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("Name is mandatory.")
                .NotEmpty().WithMessage("Product name is mandatory.")
                .Length(3, 150).WithMessage("The name must be between 3 and 150 characters long.");

            RuleFor(p => p.UnitPrice)
               .NotNull().WithMessage("Unit price must not be empty.")
               .NotEmpty().WithMessage("Unit price must not be empty.")
               .Must(unitPrice => decimal.TryParse(unitPrice, out var result) && result >= 0)
               .WithMessage("Unit price must be a valid decimal value and non-negative.");

            RuleFor(p => p.Quantity)
                .NotNull().WithMessage("Quantity is mandatory.")
                .NotEmpty().WithMessage("Quantity is mandatory.")
                .Must(quantity => int.TryParse(quantity, out var result) && result >= 1)
                .WithMessage("Quantity must be a valid integer and greater than or equal to one.");

            RuleFor(p => p.UserId)
       .        NotNull().WithMessage("UserId is mandatory.")
       .        Must(id => Guid.TryParse(id, out _))
       .        WithMessage("UserId must be a valid GUID.");
        }
    }
}
