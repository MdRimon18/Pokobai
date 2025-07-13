using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Accounts
{
    public class AccountsDailyExpanse
    {
        public long AccDailyExpanseId { get; set;}  
        public Guid? AccDailyExpanseKey { get; set;}
        [Required(ErrorMessage = "AccHead Name is required")]
        public int? AccHeadId { get; set; }
        public decimal? Expense { get; set; } 
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Remarks is required")]
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        public DateTime? EntryDateTime { get; set;}
        public long? EntryBy { get; set; } = null;
        public DateTime? LastModifyDate { get; set;} 
        public long? LastModifyBy { get; set;} 
        public DateTime? DeletedDate { get; set;} 
        public long? DeletedBy { get; set;} 
        public string Status { get; set;} 
        public int SuccessOrFailId { get; set;}
        public int total_row { get; set; }
    }
}
