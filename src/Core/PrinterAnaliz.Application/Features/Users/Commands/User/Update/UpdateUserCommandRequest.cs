using MediatR;
using Microsoft.AspNetCore.Http;
using PrinterAnaliz.Application.Views.UserRoleViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Update
{
    public class UpdateUserCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? OldPassword { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public bool? EmailNotification { get; set; }
        public bool? CanOrder { get; set; } = true;
        public IFormFile ProfileImage { get; set; }
        public IList<int>? RoleId { get; set; }
        public IList<long>? CustomerId { get; set; }
    }
}
