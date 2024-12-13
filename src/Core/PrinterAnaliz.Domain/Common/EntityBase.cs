using System.ComponentModel.DataAnnotations.Schema;

namespace PrinterAnaliz.Domain.Common
{
    public class EntityBase : IEntityBase
    {
        public long Id { get; set; }
        public string CreateFrom { get; set; }
        public string? UpdateFrom { get; set; }
        public string? DeletedFrom { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
 
    }


}
