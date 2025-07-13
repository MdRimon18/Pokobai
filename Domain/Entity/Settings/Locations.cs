using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Settings
{
	public class Locations
	{
		public long LocationId { get; set; }
		public Guid? LocationKey { get; set; }
        [Required(ErrorMessage = " Location Name is required")]
        [StringLength(100, ErrorMessage = " Location Name must not exceed 100 characters")]
        public string LocationName { get; set; }
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
