using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerOrdersService.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
