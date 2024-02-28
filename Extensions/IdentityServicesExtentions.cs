using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Serivecs;
using Talabat.Repositry.Identity;
using Talabt.Service;

namespace Talabat.APis.Extensions
{
    public static class IdentityServicesExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {

            Services.AddScoped<ITokenSerivec, TokenSerivec>();
          
            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
               options.Password.RequireDigit= true;
            })
                .AddEntityFrameworkStores<AppIdentityDBContext>();

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(options =>
                             {
                                options.TokenValidationParameters = new TokenValidationParameters()
                                 {
                                     ValidateIssuer = true,
                                     ValidIssuer = configuration["Jwt:ValidIssuer"],
                                     ValidateAudience = true,
                                     ValidAudience = configuration["Jwt:ValidAudience"],
                                     ValidateLifetime = true,//DUratioDat
                                     ValidateIssuerSigningKey = true,
                                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                                 };
                             }
                             );
            return Services;
        }
    }
}
