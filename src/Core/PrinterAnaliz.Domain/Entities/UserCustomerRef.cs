using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class UserCustomerRef : EntityBase
    {
   
        public long UserId { get; set; }
        public long CustomerId { get; set; }
    }
}
