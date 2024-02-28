using Microsoft.AspNetCore.Mvc;
using Talabat.APis.Errors;
using Talabat.APis.Helper;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Serivecs;
using Talabat.Repositry;
using Talabat.Service;
using Talabt.Service;

namespace Talabat.APis.Extensions
{
    public static class  ApplicationServicesExtension
    {
        public static  IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {

            Services.AddSingleton<IResponseCacheService,ResponseCacheService>();
            Services.AddScoped<IPaymentService,PaymentService>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<IOrderSerice, OrderSerive>();
           Services.AddScoped<IUnitOfWork, UnitOfWork>();
           Services.AddAutoMapper(typeof(MappingProfilies));
       Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(E => E.ErrorMessage)
                                                        .ToArray();
                    var vaildationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(vaildationErrorResponse);
                };


            });

            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            return Services;
        }
    }
}
