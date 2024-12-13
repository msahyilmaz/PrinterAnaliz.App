using FluentValidation;
using PrinterAnaliz.Application.Enums;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Create
{
    public class CreateAddressesCommandValidator:AbstractValidator<CreateAddressesCommandRequest>
    {
        public CreateAddressesCommandValidator()
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("Kullanıcı id boş olamaz.");
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Adres başlığı boş olamaz.");

            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("Ad boş olamaz.");

            RuleFor(x => x.Surname)
               .NotEmpty()
               .WithMessage("Soyad boş olamaz.");

            RuleFor(x => x.Phone)
               .NotEmpty()
               .WithMessage("Telefon boş olamaz.");

            RuleFor(x => x.AddressDescription)
               .NotNull()
               .WithMessage("Adres açıklaması boş olamaz.");
            RuleFor(x => x.AddressType)
              .Must(i => Enum.IsDefined(typeof(AddressesTypes), i))
              .WithMessage("Adres tipi geçerli değil.");
            RuleFor(x => x.TaxNumber)
               .NotEmpty()
               .When(x=>x.AddressType == Enums.AddressesTypes.Institutional)
               .WithMessage("Vergi Numarası boş olamaz.");
        }
    }
}
