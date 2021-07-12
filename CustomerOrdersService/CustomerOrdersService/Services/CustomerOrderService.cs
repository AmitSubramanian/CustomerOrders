using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerOrdersService.Dtos.Read;
using CustomerOrdersService.Dtos.Create;
using CustomerOrdersService.Models;

namespace CustomerOrdersService.Services
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly IMapper _mapper;
        private readonly CustomerOrderContext _customerOrderContext;

        public CustomerOrderService(IMapper mapper, CustomerOrderContext customerOrderContext)
        {
            _mapper = mapper;
            _customerOrderContext = customerOrderContext;
        }

        public List<ReadCustomerDto> GetCustomers()
        {
            return _customerOrderContext.Customers
                .Select(c => new ReadCustomerDto()
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToList();
        }

        public ReadCustomerOrdersDto GetOrdersByCustomer(int customerId)
        {
            var customer = _customerOrderContext.Customers.Include(x => x.Orders)
                .Where(x => x.CustomerId == customerId)
                .Select(x => x)
                .FirstOrDefault();

            return _mapper.Map<Customer, ReadCustomerOrdersDto>(customer);
        }

        public int CreateCustomer(CreateCustomerDto customerDto)
        {
            var customer = _mapper.Map<CreateCustomerDto, Customer>(customerDto);

            _customerOrderContext.Customers.Add(customer);
            _customerOrderContext.SaveChanges();

            return customer.CustomerId;
        }

        public int CreateOrder(int customerId, CreateOrderDto orderDto)
        {
            var order = _mapper.Map<CreateOrderDto, Order>(orderDto);
            order.CustomerId = customerId;

            _customerOrderContext.Orders.Add(order);
            _customerOrderContext.SaveChanges();

            return order.OrderId;
        }

        // Additional method
        public ReadCustomerDto GetCustomerById(int customerId)
        {
            var customer = _customerOrderContext.Customers.Find(customerId);

            return _mapper.Map<Customer, ReadCustomerDto>(customer);
        }

        // Additional method
        public ReadOrderAllFieldsDto GetOrderById(int orderId)
        {
            var order = _customerOrderContext.Orders.Find(orderId);

            return _mapper.Map<Order, ReadOrderAllFieldsDto>(order);
        }

        // Additional method
        public bool DeleteCustomer(int customerId)
        {
            var customer = _customerOrderContext.Customers.Find(customerId);

            if (customer == null)
                return false;

            _customerOrderContext.Remove(customer);
            _customerOrderContext.SaveChanges();

            return true;
        }

        // Additional method
        public bool DeleteOrder(int orderId)
        {
            var order = _customerOrderContext.Orders.Find(orderId);

            if (order == null)
                return false;

            _customerOrderContext.Remove(order);
            _customerOrderContext.SaveChanges();

            return true;
        }
    }
}
