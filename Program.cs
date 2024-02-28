using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using Talabat.APis.Errors;
using Talabat.APis.Extensions;
using Talabat.APis.Helper;
using Talabat.APis.Middleswares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repositry;
using Talabat.Repositry.Data;
using Talabat.Repositry.Identity;

namespace Talabat.APis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers(); // we add servcir for Api
                                               // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerSerivces();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            }) ;
            builder.Services.AddDbContext<AppIdentityDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // any serivecs of bussines is 99% is a Scopaed
           builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            #endregion

            var app = builder.Build();
            #region Update DataBase
            // we want to ask clr to creat object Explicity
          using  var Scope = app.Services.CreateScope(); // is a contioer for a sericve that added in webapp in serice setion we seletcscoped
            var Services = Scope.ServiceProvider;// to add dependancy to scopedswr
            var Loggerfactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbcontext = Services.GetRequiredService<StoreDbContext>();// ask from clr impplecty 
                await dbcontext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(dbcontext);
                var IdnetityDBContext = Services.GetRequiredService<AppIdentityDBContext>();
                await IdnetityDBContext.Database.MigrateAsync();
                var DataSeedUserManger = Services.GetRequiredService<UserManager<AppUser>>(); // ask object implicty
                await IdentityDbContextSeed.SeedAsync(DataSeedUserManger);

            }
            catch (Exception ex)
            {
                // we cath ecp with looging 
                var logger=Loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Accured throw apply Migration");
            }

            #endregion

            #region MiddleWares
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();// is take a route for contr and coma if math wih url =UseEndpio 
                #endregion

            app.Run();
        }
    }
}