using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class PrinterLog : EntityBase
    {
        public long PrinterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal JobRate { get; set; }
        public decimal SquareMeters { get; set; }
        public string? JobFileName { get; set; }
    }
}
