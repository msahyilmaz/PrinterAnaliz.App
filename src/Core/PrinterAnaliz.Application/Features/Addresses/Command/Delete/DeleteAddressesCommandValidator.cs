using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Delete
{
    public class DeleteAddressesCommandValidator:AbstractValidator<DeleteAddressesCommandRequest>
    {
        public DeleteAddressesCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
                
        }
    }
}
