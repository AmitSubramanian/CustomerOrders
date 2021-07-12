using System;

namespace CustomerOrdersService.Dtos.Read
{
    public class ReadOrderDto
    {
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
