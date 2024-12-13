using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Printers.Command.Create
{
    public class CreatePrinterCommandValidator:AbstractValidator<CreatePrinterCommandRequest>
    {
        public CreatePrinterCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Müşteri numarası olamdan yazıcı eklenemez.");
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Yazıcı adı boş bırakılamaz.");
        }
    }
}
