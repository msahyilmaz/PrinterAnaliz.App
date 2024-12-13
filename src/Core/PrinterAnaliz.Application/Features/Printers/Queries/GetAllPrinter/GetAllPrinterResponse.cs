namespace PrinterAnaliz.Application.Features.Printers.Queries.GetAllPrinter
{
    public class GetAllPrinterResponse
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrinterActive { get; set; }
        public bool IsClientActive { get; set; }
    }
}
