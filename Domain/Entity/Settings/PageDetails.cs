namespace Domain.Entity.Settings
{
    public class PageDetails
    {
        public long PageId { get; set; }   
        public Guid PageKey { get; set; } 
        public string PageName { get; set; }  
        public string Status { get; set; }
        public int total_row { get; set; }
    }
}
