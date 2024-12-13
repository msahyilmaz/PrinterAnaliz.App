using MediatR;

namespace PrinterAnaliz.Application.Features.Printers.Command.Create
{
    public class CreatePrinterCommandRequest:IRequest<long>
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
