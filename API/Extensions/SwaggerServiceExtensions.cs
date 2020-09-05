using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            //and we also have to add this as a middleware
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{Title = "SkiNet API", Version = "v1"});
            });

            return services;
        }
        //we also will create an extension method for the middleware part of the swagger Configuration
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            //with these two pieces of middleware we are using swagger and swaggerUI which allows us to browse a web page which is gonna show all of our API Endpoints
            app.UseSwagger();
            app.UseSwaggerUI(c => {c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1");});

            return app;
        }
    }
}