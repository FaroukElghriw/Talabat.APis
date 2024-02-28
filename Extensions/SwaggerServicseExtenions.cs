
 namespace Talabat.APis.Extensions
{
    public static class SwaggerServicseExtenions
    {
        public  static IServiceCollection AddSwaggerSerivces(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();// swagger
          Services.AddSwaggerGen();// is a serci for swwagr 
            return Services;
        }
        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {

            app.UseSwagger();// for make docs for api
            app.UseSwaggerUI();// for use docs and ui
            return app;
        }
    }
}
