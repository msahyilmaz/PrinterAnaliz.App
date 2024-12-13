using PrinterAnaliz.Application.Enums;

namespace PrinterAnaliz.Application.Extensions
{
    public class LoginedUserModel
    {
      
        public long Id { get; set; }
        public string userName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailEmail { get; set; }
        public IList<UserRoleTypes> UserRoles { get; set; }
        public IList<long> Customers { get; set; }
    }
}
