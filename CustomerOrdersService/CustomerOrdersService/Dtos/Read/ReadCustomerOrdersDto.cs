using System.Collections.Generic;

namespace CustomerOrdersService.Dtos.Read
{
    public class ReadCustomerOrdersDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<ReadOrderDto> Orders { get; set; }
    }
}
