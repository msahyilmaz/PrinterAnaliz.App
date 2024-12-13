using FluentValidation;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Create
{
    public class CreateUserCommandRequestValidator:AbstractValidator<CreateUserCommandRequest>
    {
        public CreateUserCommandRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Kullanıcı adı boş olamaz.");

            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("Ad boş olamaz.");

            RuleFor(x => x.Surname)
               .NotEmpty()
               .WithMessage("Soyad boş olamaz.");

            RuleFor(i => i.Email)
                .NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("İlgili adres geçerli bir e-posta adresi değil.");

            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifreniz boş olamaz")
                    .MinimumLength(6).WithMessage("Şifre uzunluğunuz en az 6 karakter olmalıdır.")
                    .MaximumLength(16).WithMessage("Şifre uzunluğunuz 16 karakteri geçmemelidir. Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                    .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                    .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                    .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("Şifre en az bir özel karakter içermelidir.");

            RuleFor(i => i.EmailNotification)
                .Must(x => x == false || x == true)
                .WithMessage("E-Posta bildirimi seçeneği true veya false olmalı.");

            RuleFor(i => i.CanOrder)
                .Must(x => x == false || x == true)
                .WithMessage("Sipariş verme seçeneği true veya false olmalı.");
            RuleFor(i => i.RoleId)
               .NotEmpty()
               .NotNull()
               //.Must(i => Enum.IsDefined(typeof(UserRoleTypes), i))
               .Must(i=>i.All(j => j != 0))
               .WithMessage($"Kulanıcı rolü olmadan kullanıcı eklenemez.");
           
        }
      
    }
}
