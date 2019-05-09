using Microsoft.AspNetCore.Http;
using PersonalBlog.Services.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PersonalBlog.Api.CustomMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, object exception)
        {
            httpContext.Response.ContentType = "application/json";
            if (exception.GetType() == typeof(ValidationException))
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return httpContext.Response.WriteAsync(
                    "Uncorrect request with status code 400! " +
                    $"{(exception as ValidationException).Message}"
                    );
            }
            else if (exception.GetType() == typeof(ObjectNotFoundException))
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status404NotFound;
                return httpContext.Response.WriteAsync(
                    "Uncorrect request with status code 404! " +
                    $"{(exception as ObjectNotFoundException).Message}"
                    );
            }
            else if (exception.GetType() == typeof(NotImplementedException))
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status405MethodNotAllowed;
                return httpContext.Response.WriteAsync(
                    "Invalid request with status code 405! " +
                    $"{(exception as NotImplementedException).Message}"
                    );
            }
            else if (exception.GetType() == typeof(ObjectNotFoundException))
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status404NotFound;
                return httpContext.Response.WriteAsync($"Uncorrect request with status code 404");
            }
            else
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
                return httpContext.Response.WriteAsync($"Internal server error with status code 500");
            }
        }
    }
}
