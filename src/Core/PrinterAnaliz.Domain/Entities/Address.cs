using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class Address : EntityBase
    {
 

        public long UserId { get; set; }
        public long? CustomerId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string AddressDescription { get; set; }
        public string LatLng { get; set; }
        public string TaxNumber { get; set; }
        public int AddressType { get; set; }
    }
}
