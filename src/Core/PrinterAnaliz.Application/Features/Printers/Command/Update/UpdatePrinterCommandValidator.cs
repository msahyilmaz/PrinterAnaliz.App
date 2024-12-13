using FluentValidation;

namespace PrinterAnaliz.Application.Features.Printers.Command.Update
{
    public class UpdatePrinterCommandValidator:AbstractValidator<UpdatePrinterCommandRequest>
    {
        public UpdatePrinterCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Yazıcı numarası olamdan yazıcı güncellenemez.");
            RuleFor(x => x.CustomerId)
                 .NotEmpty()
                 .WithMessage("Müşteri numarası olamdan yazıcı güncellenemez.");
          
        }
    }
}
