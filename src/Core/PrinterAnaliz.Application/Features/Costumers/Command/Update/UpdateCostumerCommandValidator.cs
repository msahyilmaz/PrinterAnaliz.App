using FluentValidation;

namespace PrinterAnaliz.Application.Features.Costumers.Command.Update
{
    public class UpdateCostumerCommandValidator:AbstractValidator<UpdateCostumerCommandRequest>
    {
        public UpdateCostumerCommandValidator()
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
