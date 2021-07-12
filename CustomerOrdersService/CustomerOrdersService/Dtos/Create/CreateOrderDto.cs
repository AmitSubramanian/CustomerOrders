using System;

namespace CustomerOrdersService.Dtos.Create
{
    public class CreateOrderDto
    {
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
