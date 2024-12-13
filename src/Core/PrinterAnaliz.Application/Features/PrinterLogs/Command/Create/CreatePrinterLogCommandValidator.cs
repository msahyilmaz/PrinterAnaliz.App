using FluentValidation;

namespace PrinterAnaliz.Application.Features.PrinterLogs.Command.Create
{
    public class CreatePrinterLogCommandValidator:AbstractValidator<CreatePrinterLogCommandRequest>
    {
        public CreatePrinterLogCommandValidator()
        {
            RuleFor(x => x.PrinterId).NotEmpty().WithMessage("Printer olmadan log ekleyemezsiniz.");
            RuleFor(x => x.StartDate).NotEmpty().Must(checkDateForRequest).WithMessage("İşlemin başlangıç tarihi olmadan log eklenemez.");
            RuleFor(x => x.EndDate).NotEmpty().Must(checkDateForRequest).WithMessage("İşlemin bitiş tarihi olmadan log eklenemez.");
            RuleFor(x => x.JobRate).NotEmpty().WithMessage("İşlemin bitiş oranı olmadan log eklenemez.");
            RuleFor(x => x.SquareMeters).NotEmpty().WithMessage("İşlemin toplam metrekaresi olmadan log eklenemez.");
        }
        private bool checkDateForRequest(DateTime date)
        {
            if (date == default(DateTime))
                return false;
            return true;
        }
    }
}
