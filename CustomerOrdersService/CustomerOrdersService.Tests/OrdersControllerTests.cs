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
    public class OrdersControllerTests : IDisposable
    {
        IMapper mapper;

        public OrdersControllerTests()
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
        public void GetOrderById_ReturnsSameOrder()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "get_order_by_id")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {
                var controller = new OrdersController(new CustomerOrderService(mapper, context), mapper);
                Customer testCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@a.com" };
                context.Customers.Add(testCustomer);

                Order testOrder = new Order { CustomerId = testCustomer.CustomerId, OrderId = 1, Price = 100.11M, CreatedDate = new DateTime(2019, 6, 9) };
                context.Orders.Add(testOrder);

                context.SaveChanges();

                var result = controller.GetOrderById(testOrder.OrderId) as OkObjectResult;

                var resultValue = (ReadOrderAllFieldsDto)result.Value;

                Assert.True(result != null);
                Assert.Equal(testOrder.OrderId, resultValue.OrderId);
                Assert.Equal(testOrder.Price, resultValue.Price);
                Assert.Equal(testOrder.CreatedDate, resultValue.CreatedDate);
                Assert.Equal(testCustomer.CustomerId, resultValue.CustomerId);
            }
        }

        [Fact]
        public void GetCustomerById_WhenCustomerDoesNotExist_ReturnsNotFound()
        {
            var options = new DbContextOptionsBuilder<CustomerOrderContext>()
            .UseInMemoryDatabase(databaseName: "order_does_not_exist")
            .Options;

            using (var context = new CustomerOrderContext(options))
            {
                Customer testCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@a.com" };
                context.Customers.Add(testCustomer);

                Order testOrder = new Order { CustomerId = testCustomer.CustomerId, OrderId = 1, Price = 100.11M, CreatedDate = new DateTime(2019, 6, 9) };
                context.Orders.Add(testOrder);

                context.SaveChanges();

                var controller = new OrdersController(new CustomerOrderService(mapper, context), mapper);

                var result = controller.GetOrderById(999);

                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
