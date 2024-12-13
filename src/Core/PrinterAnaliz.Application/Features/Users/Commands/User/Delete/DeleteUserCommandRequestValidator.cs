using FluentValidation;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Delete
{
    public class DeleteUserCommandRequestValidator:AbstractValidator<DeleteUserCommandRequest>
    {
        public DeleteUserCommandRequestValidator()
        {
                RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Kullanıcı Id boş olamaz.");
        }
    }
}
