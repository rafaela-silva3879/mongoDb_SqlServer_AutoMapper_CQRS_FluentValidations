using DeveloperStore.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Validation
{
    public class CreateUserModelValidation : AbstractValidator<(string Name, string Email, string Password)>
    {
        public CreateUserModelValidation()
        {
            // Name must contain only letters and accents, no numbers or special characters
            RuleFor(u => u.Name)
                .NotNull().NotEmpty().WithMessage("Name is mandatory.")
                .Length(6, 150).WithMessage("The name must be between 6 and 150 characters long.")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$").WithMessage("The name must contain only letters.");

            // Email must be valid
            RuleFor(u => u.Email)
                .NotNull().NotEmpty().WithMessage("Email is mandatory.")
                .Length(6, 50).WithMessage("The email must be between 6 and 50 characters long.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            // Password must contain uppercase, lowercase, a number, and a special character
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is mandatory.")
                .Matches(@"[A-Z]+").WithMessage("The password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("The password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("The password must contain at least one number.")
                .Matches(@"[\!\?\*\.\@]+").WithMessage("The password must contain at least one special character (!?*.@).")
                .Length(8, 50).WithMessage("The password must be between 8 and 50 characters long.");
        }
    }
}

