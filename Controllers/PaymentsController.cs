using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APis.DTOS;
using Talabat.APis.Errors;
using Talabat.Core.Serivecs;

namespace Talabat.APis.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateorUpdatePaymentIntent(string BasketId)
        {
            var basket= await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "A Problem with your basket"));
            return Ok(basket);
        }
    }
}
