
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DeveloperStore.Application.Validation
{
    public class ProductSubmitSelectionViewModelValidation : AbstractValidator<(string UserId, string ProductId, int Quantity)>
    {
        public ProductSubmitSelectionViewModelValidation()
        {
            RuleFor(p => p.Quantity)
                .NotNull().WithMessage("Quantity is mandatory.")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Quantity must be a valid integer and greater than or equal to one.");

            RuleFor(p => p.ProductId)
                .NotNull().WithMessage("ProductId is mandatory.")
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("ProductId must be a valid GUID");

            RuleFor(p => p.UserId)
                .NotNull().WithMessage("UserId is mandatory.")
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("UserId must be a valid GUID.");
        }
    }
}
