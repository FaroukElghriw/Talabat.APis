using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabat.APis.DTOS
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } // as this will be Guid
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public string? PaymentIntendId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
