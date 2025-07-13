using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Settings
{
    public class ColumPermission
    {
        public long ColumnPermssinId { get; set; }
        public Guid ColumnPermssinKey { get; set; }
        public long BranchId { get; set; }
        public long PageId { get; set; }
        [Required(ErrorMessage = "Color Name is Column Name")]
        public string ColumnName { get; set; }
        [Required(ErrorMessage = "Column Proprty Name is required")]
        public string ColumnProprtyName { get; set; }
        public string ColumnProprtyDataType { get; set; }
        [Required(ErrorMessage = "Is Searchable is required")]
        public bool IsSearchable { get; set; }
        [Required(ErrorMessage = "Column Position is required")]
        public int ColumnPosition { get; set; }
        [Required(ErrorMessage = "Is Show is required")]
        public bool IsShow { get; set; }
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
