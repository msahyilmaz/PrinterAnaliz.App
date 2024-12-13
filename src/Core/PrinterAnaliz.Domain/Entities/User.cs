using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class User : EntityBase
    {
         
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfileImage { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? ForgetPasswordQueryDate { get; set; }
        public string Email { get; set; }
        public bool EmailNotification { get; set; }
        public bool CanOrder { get; set; } = true;

    }
}
