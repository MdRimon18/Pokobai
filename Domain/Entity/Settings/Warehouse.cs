using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class Warehouse
    {
        public long WarehouseId { get; set; }
        public Guid? WarehouseKey { get; set; }
        [Required(ErrorMessage = " Warehouse Name is required")]
        [StringLength(100, ErrorMessage = " Warehouse Name must not exceed 100 characters")]
        public string WarehouseName { get; set; }
       
        public long LocationId { get; set; }
        public string ManagerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? Capacity { get; set; }
        public string OperatingHours { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public int total_row { get; set; }
    }
}
