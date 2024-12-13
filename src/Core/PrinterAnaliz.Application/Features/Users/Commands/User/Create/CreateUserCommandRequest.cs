using MediatR;
using Microsoft.AspNetCore.Http;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Create
{

    public class CreateUserCommandRequest : IRequest<long>
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool EmailNotification { get; set; }
        public bool CanOrder { get; set; } = true;
        public IFormFile? ProfileImage { get; set; }
        public IList<int> RoleId { get; set; }
        public IList<long>? CustomerId { get; set; }
    }
}
