using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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
        //we will clean our StartUp class so we will extend the IServiceCollection so that we can move some of our own services inside there , and we will do this inside Extenstions Folder 
        public void ConfigureServices(IServiceCollection services)
        {
            
            //we need to add our dataContext or our StoreContext as a service so that we can use it in other parts of our application
            //after the step of adding our DbContext as a service, we look at entityframework migrations. and this gonna create us some code so that we can scaffold our databse and create our database, it's gonna take a look inside our StoreContext and it's gonna see that we 've got a DbSet property related to Product entity and it creates the table

            services.AddControllers();

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
        
            //we are creating a new database here which is why we are specifying a separate service for this, we will have a completely separate databse for identity
            services.AddDbContext<AppIdentityDbContext>(x => {
                x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            });
            
            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            //Extension Method we have created
            services.AddApplicationServices();

            //Extension Method we have created
            services.AddIdentityServices(_config);

            //Extension Method we have created
            services.AddSwaggerDocumentation();

            //So we have one small task to go before we hand over our API to our client side developer and that's to enable cross origin resource sharing support otherwise known as cause so that we send back the appropriate header with our responses to the client and cores is a mechanism that's used to tell browsers to give a web application running at one origin access to selected resources from a different origin and for security reasons browsers restrict cross origin HTTP requests initiated from javascript.
            //So if we want to see our results coming back from the API in the browser then we're going to need to send back across origin resource sharing header to enable that to happen. It's just a header that has to be passed down to the client
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"); //and what we'll do is we'll say opt and add policy and we'll give it a name of corspolicy and then the policy will give some settings and what we want to do is say policy and allow any header and allow any methods and what we'll do is specify with origins and we're just going to specify the U.R.L. where our client is gonna be coming from "https://localhost:4200".
                                                                                                    //and we are basically telling our clients application that if it's running on an unsecured port we're not going to return any or we're not going to return a header that's going to allow our browser to display the information. so we are gonna configure our application to use HTTPS as well. 
                                                                                                    //So after we've added the service we need to add the middleware for this as well.
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            //when the request comes into our API server but we don't have an endpoint that matches that particular request, then we are gonna hit this particular middleware and it's gonna redirect to ErrorController and pass in the statusCode 
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //this middleware means that if a request comes into the server on one of its ports it's listening on. In this case http:5000, then it's going to redirect it to the https:5001 
            //          Status Code: 307 Temporary Redirect => when we request http:5000, this response is sent to the browser, our browser says ok i will go to the https address and get the resource from there
            app.UseHttpsRedirection();

            //this middleware is responsible for getting us to the controller that we are hitting
            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");  //and like I say we're not going to see much difference in postman but we should see the header coming back Once we specify the origin inside there.
            
            app.UseAuthentication();
            app.UseAuthorization();

            //Extension Method we have created
            app.UseSwaggerDocumentation();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
