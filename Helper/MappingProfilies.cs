using AutoMapper;
using Talabat.APis.DTOS;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APis.Helper
{
    public class MappingProfilies:Profile
    {
        public MappingProfilies()
        {
            //CreateMap<Product, ProductToReturnDTO>()
            //    .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
            //    .ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name));
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>();
            CreateMap<AddressDto, Talabat.Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDTo>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
