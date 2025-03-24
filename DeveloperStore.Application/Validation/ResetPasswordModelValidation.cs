using FluentValidation;
using System;

namespace DeveloperStore.Application.Validation
{
    public class ResetPasswordModelValidation : AbstractValidator<(string NewPassword, string ConfirmPassword)>
    {
        public ResetPasswordModelValidation()
        {
            RuleFor(c => c.NewPassword)
                .NotNull().NotEmpty().WithMessage("Password is required.")
                .Matches(@"[A-Z]+").WithMessage("The password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("The password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("The password must contain at least one number.")
                .Matches(@"[\!\?\*\.\@]+").WithMessage("The password must contain at least one special character (!?*.@).")
                .Length(8, 50).WithMessage("The password must be between 8 and 50 characters long.");

            RuleFor(c => c.ConfirmPassword)
                .NotNull().NotEmpty().WithMessage("Password confirmation is required.")
                .Equal(c => c.NewPassword).WithMessage("Passwords do not match.");
        }
    }
}
