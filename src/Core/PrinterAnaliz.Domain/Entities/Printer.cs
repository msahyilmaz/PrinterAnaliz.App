using PrinterAnaliz.Domain.Common;

namespace PrinterAnaliz.Domain.Entities
{
    public class Printer : EntityBase
    {
  
        public long CustomerId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsPrinterActive { get; set; } 
        public bool IsClientActive { get; set; }  
    }
}
