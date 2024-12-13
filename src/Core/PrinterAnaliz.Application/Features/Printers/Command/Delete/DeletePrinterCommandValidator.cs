using FluentValidation;

namespace PrinterAnaliz.Application.Features.Printers.Command.Delete
{
    public class DeletePrinterCommandValidator:AbstractValidator<DeletePrinterCommandRequest>
    {
        public DeletePrinterCommandValidator()
        {
            RuleFor(x => x.Id)
                   .NotEmpty()
                   .WithMessage("Yazıcı numarası olamdan yazıcı silinemez.");
        
        }
    }
}
