using System.Text.Json;                           
using Microsoft.AspNetCore.Http;                 
using Microsoft.Extensions.Logging;              
using System;                                    
using System.Threading.Tasks;





namespace HotelBookingSystem.API.MiddlWare
{
    public class ExceptionHandlingMiddleware




    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;



        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An UnHandled Excpetion occured ");

                await HandleExceptionAsync(context, ex);
            }
          
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception Exception)
        {
            context.Response.ContentType = "application/json";


            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {


                StatusCode = context.Response.StatusCode,
                Message = "An error occured while processing your request",


                Detail = Exception.Message
            };

            var json =  JsonSerializer.Serialize(response);


            return context.Response.WriteAsync(json);
            
        }

    }

}

