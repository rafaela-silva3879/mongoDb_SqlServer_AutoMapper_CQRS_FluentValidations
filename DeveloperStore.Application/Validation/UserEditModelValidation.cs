using DeveloperStore.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Validation
{
    public class UserEditModelValidation : AbstractValidator<(string Name, string Email, string UserProfile)>
    {
        public UserEditModelValidation()
        {
            // Name must contain only letters and accents, no numbers or special characters
            RuleFor(u => u.Name)
                .NotNull().NotEmpty().WithMessage("Name is mandatory.")
                .Length(6, 150).WithMessage("The name must be between 6 and 150 characters long.")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$").WithMessage("The name must contain only letters.");

            // Email must be valid
            RuleFor(u => u.Email)
                .NotNull().WithMessage("Email is mandatory.")
                .NotEmpty().WithMessage("Email is mandatory.")
                .Length(6, 50).WithMessage("The email must be between 6 and 50 characters long.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            // UserProfile
            RuleFor(u => u.UserProfile)
                .NotNull().WithMessage("User Profile is mandatory.")
                .NotEmpty().WithMessage("User Profile is mandatory.")
                .Must(value => value == "0" || value == "1")
                .WithMessage("User Profile must be either '0' (User) or '1' (Admin).");

        }
    }
}

