using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Inventory
{
    public class InvoiceItemSerials
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long InvoiceItemSerialId { get; set; }
        public long InvoiceItemId { get; set; }
        public long InvoiceId { get; set; }
        public long ProdSerialNmbrId { get; set; }
        public string SerialNumber { get; set; }
        public double Rate { get; set; }
         
    }
}
