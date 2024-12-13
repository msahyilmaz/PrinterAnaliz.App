using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class UserRoles: EntityBase
    {
   
        public long UserId { get; set; }
        public int RoleId { get; set; }
    }
}
