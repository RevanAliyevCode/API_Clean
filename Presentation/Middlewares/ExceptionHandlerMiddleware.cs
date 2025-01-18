using System;
using Business.Wrappers;
using Domain.Exceptions;

namespace Presentation.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = new ResponseError();

            switch (ex)
            {
                case ValidationException e:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Errors = e.Errors;
                    break;
                case NotFoundException e:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.Errors = e.Errors;
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Errors.Add("An error occured");
                    break;
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
