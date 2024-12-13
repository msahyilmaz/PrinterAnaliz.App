using MediatR;
namespace PrinterAnaliz.Application.Features.Customers.Command.Create
{
    public class CreateCustomerCommandRequest:IRequest<long>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string LogoImage { get; set; }
        public string AccountantId { get; set; }
    }
}
