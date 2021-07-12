using System;

namespace CustomerOrdersService.Dtos.Read
{
    public class ReadOrderAllFieldsDto
    {
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CustomerId { get; set; }
    }
}
