namespace Domain.Entity.Settings
{
    public class SmsSettings
    {
        public long SmsSttngId { get; set; }
        public Guid SmsSttngKey { get; set; }
        public long BranchId { get; set; }
        public long? SmsTypeId { get; set; }
        public string Title { get; set; }
        public string SenderName { get; set; }
        public string SmsCode { get; set; }
        public string ApiUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PortNumber { get; set; }
        public bool IsDefault { get; set; }
        public string Remarks { get; set; }
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
