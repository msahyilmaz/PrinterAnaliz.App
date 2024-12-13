using FluentValidation;

namespace PrinterAnaliz.Application.Features.Auth.Command.ForgetPassword
{
    public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(x => x.EmailOrUserName).NotEmpty(); 
            RuleFor(x => x.PasswordResetUrl).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.PasswordResetUrl)).WithMessage("Geçerli bir url adresi girmelisiniz.");
        }
    }
    
}
