using PrinterAnaliz.Application.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles
{
    public class GetAllUserRolesQueryResponse
    {
     
        public long UserId { get; set; }
        public int RoleId { get; set; }
        [NotMapped]
        public string RoleName 
        { 
            get { return Enum.GetName(typeof(UserRoleTypes), RoleId) ?? ""; }
            set { return; } 
        }


    }
}
