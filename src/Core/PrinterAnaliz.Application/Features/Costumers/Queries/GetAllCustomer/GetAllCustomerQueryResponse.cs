namespace PrinterAnaliz.Application.Features.Costumers.Queries.GetAllCustomer
{
    public class GetAllCustomerQueryResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string LogoImage { get; set; }
        public string AccountantId { get; set; }
    }
}
