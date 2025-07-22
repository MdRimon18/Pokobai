using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class OrderStage
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string OrderStageName { get; set; }//( 1.Order Received 2. Order Collected 3. Items Is Progress 4. Delivered 5. Order Complete 
        public long? OrderStageStaticId { get; set; }
    }
}
