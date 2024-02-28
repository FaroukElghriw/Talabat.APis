using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Talabat.APis.DTOS;
using Talabat.Core.Entities;
using static System.Net.WebRequestMethods;

namespace Talabat.APis.Helper
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["BaseApiUrl"]}{source.PictureUrl}"; 

            return string.Empty ;
        }
    }
}
