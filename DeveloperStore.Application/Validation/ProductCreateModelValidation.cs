using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace DeveloperStore.Application.Validation
{
    public class ProductCreateModelValidation : AbstractValidator<(string Name, string UnitPrice, string Quantity, IFormFile PhotoJPGJPEG)>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public ProductCreateModelValidation()
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

            RuleFor(p => p.PhotoJPGJPEG)
                .NotNull().WithMessage("Photo is required.")
                .Must(file => _allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Only JPG and JPEG files are allowed.")
                .Must(file => file.Length <= MaxFileSize)
                .WithMessage("File size must not exceed 5MB.");
        }
    }
}
