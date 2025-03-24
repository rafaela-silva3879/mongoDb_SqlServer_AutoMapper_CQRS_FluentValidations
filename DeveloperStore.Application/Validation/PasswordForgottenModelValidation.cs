using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Validation
{
    public class PasswordForgottenModelValidation : AbstractValidator<(string Email, string ConfirmEmail)>
    {
        public PasswordForgottenModelValidation()
        {
            // Email must be valid
            RuleFor(u => u.Email)
                .NotNull().WithMessage("Please, enter a valid e-mail.")
                .NotEmpty().WithMessage("Email cannot be empty.")
                .Length(6, 50).WithMessage("The email must be between 6 and 50 characters long.")
                .EmailAddress().WithMessage("Enter a valid email.");


            RuleFor(c => c.ConfirmEmail)
                .NotNull().WithMessage("Please, confirm your e-mail.")
                .NotEmpty().WithMessage("E-mail confirmation is required.")
                .Equal(c => c.Email).WithMessage("E-mails do not match.");
        }
    }
}
