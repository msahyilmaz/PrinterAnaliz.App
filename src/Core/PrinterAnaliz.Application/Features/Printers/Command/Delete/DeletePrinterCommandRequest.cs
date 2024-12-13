using MediatR;

namespace PrinterAnaliz.Application.Features.Printers.Command.Delete
{
    public class DeletePrinterCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
    }
}
