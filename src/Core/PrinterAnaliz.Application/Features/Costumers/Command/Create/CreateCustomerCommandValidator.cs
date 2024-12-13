using FluentValidation;

namespace PrinterAnaliz.Application.Features.Customers.Command.Create
{
    public class CreateCustomerCommandValidator:AbstractValidator<CreateCustomerCommandRequest>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(i => i.Email)
               .NotNull()
               .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).When(w => !String.IsNullOrEmpty(w.Email))
               .WithMessage("İlgili adres geçerli bir e-posta adresi değil.");
        }
    }
}
