using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerOrdersService.Dtos.Read;
using CustomerOrdersService.Dtos.Create;
using CustomerOrdersService.Services;
using AutoMapper;

namespace CustomerOrdersService.Controllers
{
    [Produces("application/json")]
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerOrderService _customerOrderService;
        private IMapper _mapper;

        public CustomersController(ICustomerOrderService customerOrderService, IMapper mapper)
        {
            _customerOrderService = customerOrderService;
            _mapper = mapper;
        }

        // GET api/customers
        /// <summary>
        /// Returns all customers.
        /// </summary>
        [HttpGet]
        public ActionResult<List<ReadCustomerDto>> GetCustomers()
        {
            return _customerOrderService.GetCustomers();
        }

        // GET api/customers/1/orders
        /// <summary>
        /// Gets all orders for a Customer Id.
        /// </summary>   
        [HttpGet("{customerId:int:min(0)}/orders")]
        [ProducesResponseType(typeof(ReadCustomerOrdersDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrdersByCustomer(int customerId)
        {
            ReadCustomerOrdersDto customerOrders = _customerOrderService.GetOrdersByCustomer(customerId);

            if (customerOrders == null)
                return NotFound();

            return Ok(customerOrders);
        }

        // POST api/customers
        /// <summary>
        /// Creates a customer.
        /// </summary>
        /// <param name="customerDto">Details of customer that is to be created</param>   
        [HttpPost]
        [ProducesResponseType(typeof(ReadCustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostCustomer([FromBody] CreateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var id = _customerOrderService.CreateCustomer(customerDto);

            var dto = new ReadCustomerDto()
            {
                CustomerId = id,
                Name = customerDto.Name,
                Email = customerDto.Email
            };

            return CreatedAtAction(nameof(GetCustomerById), new { customerId = id }, dto);
        }

        // POST api/customers/1/orders
        /// <summary>
        /// Creates an order for a specific customer.
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <param name="orderDto">Details of order that is to be created</param>
        [HttpPost("{customerId:int:min(0)}/orders")]
        [ProducesResponseType(typeof(ReadOrderAllFieldsDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostOrder(int customerId, [FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var id = _customerOrderService.CreateOrder(customerId, orderDto);

            var dto = new ReadOrderAllFieldsDto()
            {
                OrderId = id,
                Price = orderDto.Price,
                CreatedDate = orderDto.CreatedDate,
                CustomerId = customerId
            };

            return CreatedAtAction("GetOrderById", "Orders", new { orderId = id }, dto);
        }

        // Additional method:  Called by PostCustomer() to return 201 link.
        // GET api/customers/1
        /// <summary>
        /// Gets customer details for the given Customer Id.
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        [HttpGet("{customerId:int:min(0)}")]
        [ProducesResponseType(typeof(ReadCustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCustomerById(int customerId)
        {
            ReadCustomerDto customer = _customerOrderService.GetCustomerById(customerId);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // Additional method (to delete customer for testing).
        /// <summary>
        /// Deletes a specific customer and its orders.
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        [HttpDelete("{customerId:int:min(0)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCustomer(int customerId)
        {
            if (!_customerOrderService.DeleteCustomer(customerId))
                return NotFound();

            return NoContent();
        }
    }
}
