using FluentValidation;

namespace PrinterAnaliz.Application.Features.Auth.Command.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommandRequest>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Kullanıcı adı boş olamaz.");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifreniz boş olamaz")
                   .MinimumLength(6).WithMessage("Şifre uzunluğunuz en az 6 karakter olmalıdır.")
                   .MaximumLength(16).WithMessage("Şifre uzunluğunuz 16 karakteri geçmemelidir. Your password length must not exceed 16.")
                   .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                   .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                   .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                   .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("Şifre en az bir özel karakter içermelidir.");


        }
    }
}
