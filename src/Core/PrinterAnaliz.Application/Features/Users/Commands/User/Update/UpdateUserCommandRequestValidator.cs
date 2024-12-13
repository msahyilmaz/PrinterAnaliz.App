using FluentValidation;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Update
{
    public class UpdateUserCommandRequestValidator : AbstractValidator<UpdateUserCommandRequest>
    {
        public UpdateUserCommandRequestValidator()
        {
            RuleFor(x => x.Id) 
                .NotEmpty()
                .WithMessage("Kullanıcı Id boş olamaz.");
            
            RuleFor(x => x.UserName)
              .NotEmpty()
              .WithMessage("Kullanıcı adı boş olamaz.")
              .When(x=>x.UserName!=null);
                 
            RuleFor(x => x.Name != null)
                 .NotEmpty()
                 .WithMessage("Ad boş olamaz.")
                 .When(x => x.Name != null);

            RuleFor(x => x.Surname != null)
               .NotEmpty()
               .WithMessage("Soyad boş olamaz.").When(x => x.Surname != null);

            RuleFor(i => i.Email)
                .NotEmpty()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("İlgili adres geçerli bir e-posta adresi değil.").When(x => x.Email != null);

            RuleFor(p => p.OldPassword).NotEmpty().WithMessage("Eski Şifreniz boş olamaz")
                .MinimumLength(6).WithMessage("Şifre uzunluğunuz en az 6 karakter olmalıdır.")
                .MaximumLength(16).WithMessage("Şifre uzunluğunuz 16 karakteri geçmemelidir. Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("Şifre en az bir özel karakter içermelidir.").When(x => x.Password != null);

            RuleFor(p => p.Password).NotEmpty().WithMessage("Yeni Şifreniz boş olamaz")
                    .MinimumLength(6).WithMessage("Şifre uzunluğunuz en az 6 karakter olmalıdır.")
                    .MaximumLength(16).WithMessage("Şifre uzunluğunuz 16 karakteri geçmemelidir. Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                    .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                    .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                    .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("Şifre en az bir özel karakter içermelidir.").When(x => x.Password != null);

            RuleFor(i => i.EmailNotification)
                .Must(x => x == false || x == true)
                .WithMessage("E-Posta bildirimi seçeneği true veya false olmalı.").When(x => x.EmailNotification != null);

            RuleFor(i => i.CanOrder)
                .Must(x => x == false || x == true)
                .WithMessage("Sipariş verme seçeneği true veya false olmalı.").When(x => x.CanOrder != null);
            RuleFor(i => i.ProfileImage)
               .Must(x => ((x.Length/1024f) /1024f)< 2)
               .WithMessage(x =>$"Dosya boyutu 2mb fazla olamaz. {((x.ProfileImage.Length / 1024f) / 1024f)}").When(x => x.ProfileImage != null);
            RuleFor(i => i.ProfileImage)
             .Must(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png"))
             .WithMessage("Dosya formatı jpeg, jpg veya png olmalıdır.").When(x => x.ProfileImage != null);

           /* RuleFor(i => i.RoleId)
               .NotEmpty()
               .NotNull()
               //.Must(i => Enum.IsDefined(typeof(UserRoleTypes), i))
               .Must(i => i.All(j => j != 0))
               .WithMessage($"Kulanıcı rolü olmadan kullanıcı eklenemez.").When(x => x != null);*/

        }
    }
}
