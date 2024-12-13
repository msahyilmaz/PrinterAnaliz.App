namespace PrinterAnaliz.Application.Features.PrinterLogs.Queries.GetAllPrinterLogs
{
    public class GetAllPrinterLogsQueryResponse
    {
        public long PrinterId { get; set; }
        public string PrinterName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal JobRate { get; set; }
        public decimal SquareMeters { get; set; }
        public decimal TotalSquareMeters { get; set; }
        public string? JobFileName { get; set; }
        public bool IsPrinterActive { get; set; }
        public bool IsClientActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
