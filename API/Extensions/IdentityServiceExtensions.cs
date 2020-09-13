using Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, services);

            builder.AddEntityFrameworkStores<AppIdentityDbContext>();   //Identity doesn't have to be used with EntityFramework, it can be used with other Object Relational Mapper but we are here focusing on EntityFramework

            builder.AddSignInManager<SignInManager<AppUser>>();

            //SignInManager relies on our Authentication Service being added as well so we added this line otherwise we will see an error
            //we will configure our authentication service So we're going to tell it what type of authentication we're using and how it needs to validate the token that we're going to pass to our clients.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),

                    //then we tell it what a valid issuer is, we are only going to trust tokens that are issued from this particular server,so we are going tell it what the valid Issuer is, and because we added this to the token when we generatd the token we need this to be the same a sour token issuer that we are going to add our configuration here so:
                    ValidIssuer = config["Token:Issuer"],    //a token can have an issuer who issued the token and also it can ahve an audience who the token was issued to. And what we can also check to see is that we said that we are only going to accept the token issued by this particular server, we also will only accept the token from this particular audience and our audience will be our client application. the domain that's being hosted on 
                    ValidateIssuer = true,
                    ValidateAudience = false    //here we have overriden the default configuration and tell it to not validate the audience
                };
            });
            return services;
        }
    }
}