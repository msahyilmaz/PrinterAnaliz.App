using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class Customer : EntityBase
    {
      

        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string Email { get; set; }
        public string? LogoImage { get; set; }
        public string? AccountantId { get; set; }
       /* public virtual IList<User> Users { get; set; }
        public virtual IList<Address> Addresses { get; set; }*/
    }
}
