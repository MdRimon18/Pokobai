using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
    public class StatusSetting
    {
        public long StatusSettingId { get; set; }
        public Guid? StatusSettingKey { get; set; }  
        public long BranchId { get; set; }
        [Required(ErrorMessage = "Status Setting Name is required")]
        [StringLength(100, ErrorMessage = "StatusSetting Name must not exceed 100 characters")]
        public string StatusSettingName { get; set; }
        [Required(ErrorMessage = "Status Setting Name is required")]
        [StringLength(100, ErrorMessage = "StatusSetting Name must not exceed 100 characters")]
        public string PageName { get; set; }
        public int? Position { get; set; }
        public DateTime EntryDateTime { get; set; }
        public long EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        public int total_row { get; set; }
    }
}

