using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Xunit;
using AutoMapper;
using CustomerOrdersService.Controllers;
using CustomerOrdersService.Dtos.Create;
using CustomerOrdersService.Dtos.Read;
using CustomerOrdersService.Infrastructure;
using CustomerOrdersService.Models;
using CustomerOrdersService.Services;

namespace CustomerOrdersService.Tests
{
    public class CustomersControllerTests : IDisposable
    {
        IMapper mapper;

        public CustomersControllerTests()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddMaps(Assembly.GetAssembly(typeof(CustomerOrdersMappingProfile)));
            });
            mapper = config.CreateMapper();
        }

        public void Dispose()
        { }
        
        CreateCustomerDto GetTestCustomer()
        {
            return new CreateCustomerDto { Name = "John Doe", Email = "john.doe@a.com" };
        }

        [Fact]
        public void PostCustomer_ReturnsSameCustomer()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "post_customer")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {
                var controller = new CustomersController(new CustomerOrderService(mapper, context), mapper);
                var customerDto = GetTestCustomer();

                var result = controller.PostCustomer(customerDto) as CreatedAtActionResult;

                var resultValue = (ReadCustomerDto)result.Value;

                Assert.True(result != null);
                Assert.Equal("GetCustomerById", result.ActionName);
                Assert.Equal(result.RouteValues["customerId"], resultValue.CustomerId);
                Assert.Equal(customerDto.Name, resultValue.Name);
                Assert.Equal(customerDto.Email, resultValue.Email);
            }
        }

        [Fact]
        public void GetCustomers_ReturnsAllCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "get_customers")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {
                context.Customers.Add(new Customer { Name = "John Doe", Email = "john.doe@a.com" });
                context.Customers.Add(new Customer { Name = "Albert Einstein", Email = "albert.einstein@b.com" });
                context.Customers.Add(new Customer { Name = "Isaac Newton", Email = "isaac.newton@c.com" });
                context.SaveChanges();

                var controller = new CustomersController(new CustomerOrderService(mapper, context), mapper);

                var result = controller.GetCustomers() as ActionResult<List<ReadCustomerDto>>;

                Assert.True(result != null);
                Assert.Equal(3, result.Value.Count);
            }
        }

        [Fact]
        public void GetCustomerById_ReturnsSameCustomer()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
                .UseInMemoryDatabase(databaseName: "get_customer_by_id")
                .Options;

            using (var context = new CustomerOrderContext(options))
            {
                Customer testCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@a.com" };
                context.Customers.Add(testCustomer);
                context.SaveChanges();

                var controller = new CustomersController(new CustomerOrderService(mapper, context), mapper);

                var result = controller.GetCustomerById(testCustomer.CustomerId) as OkObjectResult;

                var resultValue = (ReadCustomerDto)result.Value;

                Assert.True(result != null);
                Assert.Equal(testCustomer.CustomerId, resultValue.CustomerId);
                Assert.Equal(testCustomer.Name, resultValue.Name);
                Assert.Equal(testCustomer.Email, resultValue.Email);
            }
        }

        [Fact]
        public void GetCustomerById_WhenCustomerDoesNotExist_ReturnsNotFound()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "customer_does_not_exist")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {

                Customer testCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@a.com" };
                context.Customers.Add(testCustomer);
                context.SaveChanges();

                var controller = new CustomersController(new CustomerOrderService(mapper, context), mapper);

                var result = controller.GetCustomerById(999);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public void GetOrdersByCustomer_ReturnsSameCustomerWithOrders()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "get_orders_by_customer")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {
                var controller = new CustomersController(new CustomerOrderService(mapper, context), mapper);
                Customer testCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@a.com" };
                context.Customers.Add(testCustomer);

                context.Orders.Add(new Order { CustomerId = testCustomer.CustomerId, Price = 100.11M, CreatedDate = new DateTime(2019, 6, 9) });
                context.Orders.Add(new Order { CustomerId = testCustomer.CustomerId, Price = 200.22M, CreatedDate = new DateTime(2019, 6, 10) });
                context.Orders.Add(new Order { CustomerId = testCustomer.CustomerId, Price = 300.33M, CreatedDate = new DateTime(2019, 6, 11) });

                context.SaveChanges();

                var result = controller.GetOrdersByCustomer(testCustomer.CustomerId) as OkObjectResult;

                var resultValue = (ReadCustomerOrdersDto)result.Value;

                Assert.True(result != null);
                Assert.Equal(testCustomer.CustomerId, resultValue.CustomerId);
                Assert.Equal(3, resultValue.Orders.Count);
            }
        }

        [Fact]
        public void PostOrder_ReturnsSameOrder()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "post_order")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {
                var controller = new CustomersController(new CustomerOrderService(mapper, context), mapper);
                var customerId = 1;
                var orderDto = new CreateOrderDto { Price = 100.11M, CreatedDate = new DateTime(2019, 6, 9) };

                var result = controller.PostOrder(customerId, orderDto) as CreatedAtActionResult;

                var resultValue = (ReadOrderAllFieldsDto)result.Value;

                Assert.True(result != null);
                Assert.Equal("GetOrderById", result.ActionName);
                Assert.Equal(result.RouteValues["orderId"], resultValue.OrderId);
                Assert.Equal(orderDto.Price, resultValue.Price);
                Assert.Equal(orderDto.CreatedDate, resultValue.CreatedDate);
                Assert.Equal(customerId, resultValue.CustomerId);
            }
        }
    }
}
