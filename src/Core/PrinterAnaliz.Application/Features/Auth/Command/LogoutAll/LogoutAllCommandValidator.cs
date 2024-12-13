using FluentValidation;
using PrinterAnaliz.Application.Features.Auth.Command.RefreshToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Auth.Command.LogoutAll
{
    public class LogoutAllCommandValidator : AbstractValidator<LogoutAllCommandRequest>
    {
        public LogoutAllCommandValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
