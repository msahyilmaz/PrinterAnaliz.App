using PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles;
using PrinterAnaliz.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfileImage { get; set; }
        public string Email { get; set; }
        public bool EmailNotification { get; set; }
        public bool CanOrder { get; set; }
        [NotMapped]
        public IList<GetAllUserRolesQueryResponse>? UserRoles { get; set; }

    }
}
