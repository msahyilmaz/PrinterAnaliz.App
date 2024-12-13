using MediatR;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Delete
{
    public class DeleteUserCommandRequest : IRequest<long>
    {
        public long Id { get; set; }
    }
}
