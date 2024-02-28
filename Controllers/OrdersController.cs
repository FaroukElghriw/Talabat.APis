using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APis.DTOS;
using Talabat.APis.Errors;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Serivecs;

namespace Talabat.APis.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderSerice _orderSerice;
        private readonly IMapper _mapper;

        public OrdersController(IOrderSerice orderSerice, IMapper mapper)
        {
            _orderSerice = orderSerice;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrderAsync(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderSerice.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order,OrderToReturnDTo>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTo>>> GetOrderForUserAsync()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderSerice.GetOrdersForUserAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDTo>>(orders));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDTo>> GetOrderForSpecificUser(int id)
        {
            var buyerEmail= User.FindFirstValue(ClaimTypes.Email);
            var order= await _orderSerice.GetOrderForUserById(id, buyerEmail);
            if(order is null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Order,OrderToReturnDTo>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethos= await _orderSerice.GetDeliveryMethodsAsync();
            return Ok(deliveryMethos);
        }

    }
}
