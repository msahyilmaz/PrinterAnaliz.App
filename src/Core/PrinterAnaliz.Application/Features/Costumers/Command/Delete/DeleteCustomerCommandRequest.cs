using MediatR;

namespace PrinterAnaliz.Application.Features.Costumers.Command.Delete
{
    public class DeleteCustomerCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
    }
}
