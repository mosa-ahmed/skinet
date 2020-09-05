using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        //IHostEnvironment is just used to check to see if we are in the development environment or not
        //we are gonna replace the middleware in the Startup class by our own this middleware, because oue Middleware is gonna handle exceptions in Development And Production environment, not just Development
        //RequestDelegate is a function that can process an HTTP request, and if there is no exception, then we want the middleware to move on to the next piece of middleware
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        //This is the Middleware method
        public async Task InvokeAsync(HttpContext context)
        {
            //because this is gonna be our Exception Handling Middleware, we will use Try{} Catch{}
            try
            {
                //this means if there is no exception, then the request moves on to its next stage
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //we also write our own response into the context response so that we can send it to the client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //and then what we can do is write out our response
                //we will get more details if we are in Development Mode
                var response = _env.IsDevelopment() ? 
                        new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                        : new ApiException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
            
        }
    }
}