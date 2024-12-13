using MediatR;

namespace PrinterAnaliz.Application.Features.PrinterLogs.Command.Create
{
    public class CreatePrinterLogCommandRequest:IRequest<long>
    {
        public long PrinterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal JobRate { get; set; }
        public decimal SquareMeters { get; set; }
        public string JobFileName { get; set; }
    }
}
