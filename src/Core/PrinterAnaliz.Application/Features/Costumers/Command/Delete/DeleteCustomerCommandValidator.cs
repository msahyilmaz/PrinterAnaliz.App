using FluentValidation;

namespace PrinterAnaliz.Application.Features.Costumers.Command.Delete
{
    public class DeleteCustomerCommandValidator:AbstractValidator<DeleteCustomerCommandRequest>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
              .NotEmpty();
        }
    }
}
