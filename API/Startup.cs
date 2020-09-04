using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//when we start our application, we create a new instance of this StartUp class and when we do this, if it has a constructor it takes a look inside here and initializes any code inside the constructor itself

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            //we need to add our dataContext or our StoreContext as a service so that we can use it in other parts of our application
            //after the step of adding our DbContext as a service, we look at entityframework migrations. and this gonna create us some code so that we can scaffold our databse and create our database, it's gonna take a look inside our StoreContext and it's gonna see that we 've got a DbSet property related to Product entity and it creates the table

            services.AddControllers();

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //this middleware means that if a request comes into the server on one of its ports it's listening on. In this case http:5000, then it's going to redirect it to the https:5001 
            //          Status Code: 307 Temporary Redirect => when we request http:5000, this response is sent to the browser, our browser says ok i will go to the https address and get the resource from there
            app.UseHttpsRedirection();

            //this middleware is responsible for getting us to the controller that we are hitting
            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
