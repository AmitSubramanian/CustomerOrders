using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerOrdersService.Dtos.Read;
using CustomerOrdersService.Dtos.Create;

namespace CustomerOrdersService.Services
{
    public interface ICustomerOrderService
    {
        List<ReadCustomerDto> GetCustomers();
        ReadCustomerOrdersDto GetOrdersByCustomer(int customerId);

        int CreateCustomer(CreateCustomerDto customer);
        int CreateOrder(int customerId, CreateOrderDto order);

        // Additional methods
        ReadCustomerDto GetCustomerById(int customerId);
        ReadOrderAllFieldsDto GetOrderById(int orderId);

        // Additional methods
        bool DeleteCustomer(int customerId);
        bool DeleteOrder(int orderId);
    }
}
