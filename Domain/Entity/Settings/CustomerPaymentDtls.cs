using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Settings
{
    public class CustomerPaymentDtls
    {
        public long CustomerPaymentId { get; set; }  // Primary Key
        public Guid? CustomerPaymentKey { get; set; }
        public long BranchId { get; set; }
        public long InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public long? EntryBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public long? LastModifyBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public int total_row { get; set; } = 0;
    }
}
