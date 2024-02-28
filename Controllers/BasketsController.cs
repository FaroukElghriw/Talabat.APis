using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APis.DTOS;
using Talabat.APis.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APis.Controllers
{
   
    public class BasketsController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository,IMapper mapper)// we should allow Dpednajecy injection
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet] // Get api/Baskets
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string id)
        {
            var basket= await _basketRepository.GetBasketAsync(id);
            return basket is null ? new CustomerBasket(id) : basket;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody]CustomerBasketDto basket)
        {
            var mappedBasket= _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var createdorupdatebasket= await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdorupdatebasket is null) return BadRequest(new ApiResponse(400));
            return Ok(createdorupdatebasket);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
