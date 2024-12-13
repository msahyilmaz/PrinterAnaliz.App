using MediatR;

namespace PrinterAnaliz.Application.Features.Printers.Command.Update
{
    public class UpdatePrinterCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsPrinterActive { get; set; }
        public bool? IsClientActive { get; set; }

    }
}
