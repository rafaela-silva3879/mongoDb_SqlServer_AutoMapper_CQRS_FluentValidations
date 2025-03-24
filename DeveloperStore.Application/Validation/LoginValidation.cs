using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Validation
{
    public class LoginValidation : AbstractValidator<(string Email, string Password)>
    {
        public LoginValidation()
        {
            // Email must be valid
            RuleFor(u => u.Email)
                .NotNull().WithMessage("Please, enter a valid e-mail.")
                .NotEmpty().WithMessage("Email cannot be empty.")
                .Length(6, 50).WithMessage("The email must be between 6 and 50 characters long.")
                .EmailAddress().WithMessage("Enter a valid email.");

            // Password must contain uppercase, lowercase, number, and special character
            RuleFor(u => u.Password)
                .NotNull().WithMessage("Please, enter a valid password.")
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"[A-Z]+").WithMessage("The password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("The password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("The password must contain at least one number.")
                .Matches(@"[\!\?\*\.\@]+").WithMessage("The password must contain at least one special character (!?*.@).")
                .Length(8, 50).WithMessage("The password must be between 8 and 50 characters long.");
        }
    }

}
