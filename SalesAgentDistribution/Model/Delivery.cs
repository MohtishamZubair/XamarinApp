using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAgentDistribution.Model 
{
 public class Delivery : BaseEntity
    {
        public Delivery():base()
        {

        }
        public int DeliverableId { get; set; }
        public int? InvoiceId { get; set; }
        public int AddressId { get; set; }
        public string AgentId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
