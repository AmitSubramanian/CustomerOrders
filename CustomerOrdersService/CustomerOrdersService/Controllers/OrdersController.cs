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
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private ICustomerOrderService _customerOrderService;
        private IMapper _mapper;

        public OrdersController(ICustomerOrderService customerOrderService, IMapper mapper)
        {
            _customerOrderService = customerOrderService;
            _mapper = mapper;
        }

        // Additional method:  Called by PostOrder() to return 201 link.
        // GET api/orders/2
        /// <summary>
        /// Gets a specific order.
        /// </summary>
        /// <param name="orderId">Order Id</param>
        [HttpGet("{orderId:int:min(0)}")]
        [ProducesResponseType(typeof(ReadOrderAllFieldsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrderById(int orderId)
        {
            ReadOrderAllFieldsDto order = _customerOrderService.GetOrderById(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // Additional method (to delete order during testing).
        /// <summary>
        /// Deletes a specific order.
        /// </summary>
        /// <param name="orderId">Order Id</param>
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteOrder(int orderId)
        {
            if (!_customerOrderService.DeleteOrder(orderId))
                return NotFound();

            return NoContent();
        }
    }
}
