using AutoMapper;
using Talabat.APis.DTOS;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APis.Helper
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["BaseApiUrl"]}{source.Product.PictureUrl}";

            return string.Empty;
        }
    }
}
