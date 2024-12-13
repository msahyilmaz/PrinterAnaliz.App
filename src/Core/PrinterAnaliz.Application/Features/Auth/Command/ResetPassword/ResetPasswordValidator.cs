using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Auth.Command.ResetPassword
{
    public class ResetPasswordValidator:AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.ResetToken).NotEmpty();
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifreniz boş olamaz")
                .MinimumLength(6).WithMessage("Şifre uzunluğunuz en az 6 karakter olmalıdır.")
                .MaximumLength(16).WithMessage("Şifre uzunluğunuz 16 karakteri geçmemelidir. Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("Şifre en az bir özel karakter içermelidir.");
            RuleFor(p => p.RePassword).NotEmpty().WithMessage("Şifre tekrarı boş olamaz")
                .MinimumLength(6).WithMessage("Şifre tekrarı uzunluğunuz en az 6 karakter olmalıdır.")
                .MaximumLength(16).WithMessage("Şifre tekrarı uzunluğunuz 16 karakteri geçmemelidir. Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Şifre tekrarı en az bir büyük harf içermelidir.")
                .Matches(@"[a-z]+").WithMessage("Şifre tekrarı en az bir küçük harf içermelidir.")
                .Matches(@"[0-9]+").WithMessage("Şifre tekrarı en az bir rakam içermelidir.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("Şifre tekrarı en az bir özel karakter içermelidir.")
                .Equal(p=>p.Password).WithMessage("Şifre ve şifre tekrarı uyuşmamaktadır.");
            
        }
    }
}
